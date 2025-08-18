using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }
    [SerializeField] private SfxDataContainer sfxDataContainer;
    [SerializeField] private int poolSize = 10;

    private ObjectPool<AudioSource> audioSourcePool;
    private Dictionary<int, AudioSource> _activeAudioSourcesLoops = new Dictionary<int, AudioSource>();
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Duplicated SfxManager instance found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        audioSourcePool = new ObjectPool<AudioSource>(
            CreateAudioSource,
            OnGetAudioSource,
            OnReleaseAudioSource,
            OnDestroyAudioSource,
            false,
            poolSize,
            poolSize * 2
        );
    }

    private AudioSource CreateAudioSource()
    {
        var go = new GameObject("PooledAudioSource");
        go.transform.parent = transform;
        var source = go.AddComponent<AudioSource>();
        source.playOnAwake = false;
        return source;
    }

    private void OnGetAudioSource(AudioSource source)
    {
        source.gameObject.SetActive(true);
    }

    private void OnReleaseAudioSource(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
    }

    private void OnDestroyAudioSource(AudioSource source)
    {
        if (source != null)
        {
            Destroy(source.gameObject);
        }
    }

    private IEnumerator ReleaseWhenDone(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        audioSourcePool.Release(source);
    }

    public void PlaySfx(SfxIdEnum.SfxId sfxId, Vector3 position)
    {
        var sfxData = sfxDataContainer.GetSfxData(sfxId);
        if (sfxData != null && sfxData.Clip != null)
        {
            PlaySfx(sfxData.Clip, position, sfxData.Volume, sfxData.GetRandomPitch());
        }
        else
        {
            Debug.LogWarning($"SFX with ID '{sfxId}' not found or has no clip assigned.");
        }
    }

    public void PlayLoopSfx(SfxIdEnum.loopSfxId sfxId, Vector3 postion, int instanceId = 0)
    {
        var sfxData = sfxDataContainer.GetLoopSfxData(sfxId);
        if (sfxData != null && sfxData.Clip != null)
        {
            PlayLoopSfx(sfxData.Clip, postion, sfxData.Volume, sfxData.GetRandomPitch(), instanceId);
        }
        else
        {
            Debug.LogWarning($"Loop SFX with ID '{sfxId}' not found or has no clip assigned.");
        }
    }

    private void PlaySfx(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        AudioSource source = GetAudiSourceFromPool(clip, position, volume, pitch, false);
        StartCoroutine(ReleaseWhenDone(source));
    }

    private void PlayLoopSfx(AudioClip clip, Vector3 postion, float volume = 1f, float pitch = 1f, int instanceId = 0)
    {
        if (_activeAudioSourcesLoops.ContainsKey(instanceId))
        {
            Debug.LogWarning($"Loop SFX with instance ID '{instanceId}' is already playing.");
            return;
        }

        AudioSource source = GetAudiSourceFromPool(clip, postion, volume, pitch, true);
        _activeAudioSourcesLoops.Add(instanceId, source);
    }

    public void StopLoopSfx(int instanceId)
    {
        if (_activeAudioSourcesLoops.TryGetValue(instanceId, out var source))
        {
            audioSourcePool.Release(source);
            _activeAudioSourcesLoops.Remove(instanceId);
        }
        else
        {
            Debug.LogWarning($"No active Loop SFX found with instance ID '{instanceId}'.");
        }
    }

    private AudioSource GetAudiSourceFromPool(AudioClip clip, Vector3 position, float volume, float pitch, bool loop)
    {
        var source = audioSourcePool.Get();
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.Play();
        return source;
    }

    public void UpdateLoopSfxPosition(int instanceId, Vector3 position)
    {
        if (_activeAudioSourcesLoops.TryGetValue(instanceId, out var source))
        {
            source.transform.position = position;
        }
    }
    public void SetLoopSfxPitch(int instanceId, float pitch)
    {
        if (_activeAudioSourcesLoops.TryGetValue(instanceId, out var source))
        {
            source.pitch = pitch;
        }
        else
        {
            Debug.LogWarning($"No active Loop SFX found with instance ID '{instanceId}'.");
        }
    }
}