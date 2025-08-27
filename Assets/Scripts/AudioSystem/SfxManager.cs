using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Pool;
using static SfxIdEnum;

public struct ActiveLoopSfx
{
    public Transform SourceTransform;
    public AudioSource AudioSource;
    public float DefaultVolume;

    public void UpdatePosition()
    {
        if (SourceTransform != null && AudioSource != null)
        {
            AudioSource.transform.position = SourceTransform.position;
        }
    }
}

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }
    [SerializeField] private SfxDataContainer _sfxDataContainer;
    [Header("Audio Mixer Groups")]
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;
    [SerializeField] private MusicManager _musicManager;
    private int _poolSize = 10;

    private ObjectPool<AudioSource> _audioSourcePool;
    private Dictionary<int, ActiveLoopSfx> _activeAudioSourcesLoops = new Dictionary<int, ActiveLoopSfx>();
    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Duplicated SfxManager instance found!");
            Destroy(gameObject);
            return;
        }
        Instance = this;

        _audioSourcePool = new ObjectPool<AudioSource>(
            CreateAudioSource,
            OnGetAudioSource,
            OnReleaseAudioSource,
            OnDestroyAudioSource,
            false,
            _poolSize,
            _poolSize * 2
        );

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {

    }

    private void Update()
    {
        foreach (var loopSfx in _activeAudioSourcesLoops.Values)
        {
            loopSfx.UpdatePosition();
        }
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
        _audioSourcePool.Release(source);
    }

    public void PlaySfx(SfxId sfxId, Vector3 position)
    {
        var sfxData = _sfxDataContainer.GetSfxData(sfxId);
        if (sfxData != null && sfxData.Clips != null)
        {
            StartPlaySfx(sfxData.GetAudioClip(), position, sfxData.Volume, sfxData.GetRandomPitch());
        }
        else
        {
            Debug.LogWarning($"SFX with ID '{sfxId}' not found or has no clip assigned.");
        }
    }

    public void PlayUISfx(UISfxId uISfxId)
    {
        var sfxData = _sfxDataContainer.GetUISfxData(uISfxId);
        if (sfxData != null && sfxData.Clips != null)
        {
            StartPlaySfx(sfxData.GetAudioClip(), Vector3.zero, sfxData.Volume);
        }
        else
        {
            Debug.LogWarning($"UI SFX with ID '{uISfxId}' not found or has no clip assigned.");
        }
    }

    private void StartPlaySfx(AudioClip clip, Vector3 position, float volume = 1f, float pitch = 1f)
    {
        float totalVolume = volume; // Apply current SFX volume setting
        AudioSource source = GetAudiSourceFromPool(clip, position, totalVolume, pitch, false, _sfxMixerGroup);
        StartCoroutine(ReleaseWhenDone(source));
    }

    public void PlayLoopSfx(LoopSfxId sfxId, Transform sourceTransform, int instanceId)
    {
        var sfxData = _sfxDataContainer.GetLoopSfxData(sfxId);
        if (sfxData != null && sfxData.Clips != null)
        {
            StartPlayLoopSfx(sfxData.GetAudioClip(), sourceTransform, instanceId, sfxData.Volume, sfxData.GetRandomPitch());
        }
        else
        {
            Debug.LogWarning($"Loop SFX with ID '{sfxId}' not found or has no clip assigned.");
        }
    }

    private void StartPlayLoopSfx(AudioClip clip, Transform sourceTransform, int instanceId, float volume = 1f, float pitch = 1f)
    {
        if (_activeAudioSourcesLoops.ContainsKey(instanceId))
        {
            Debug.LogWarning($"Loop SFX with instance ID '{instanceId}' is already playing.");
            return;
        }

        AudioSource source = GetAudiSourceFromPool(clip, sourceTransform.position, volume, pitch, true, _sfxMixerGroup);
        ActiveLoopSfx activeLoop = new ActiveLoopSfx
        {
            SourceTransform = sourceTransform,
            AudioSource = source,
            DefaultVolume = volume
        };
        source.volume = activeLoop.DefaultVolume; // Apply current SFX volume setting
        AddLoopSfx(instanceId, activeLoop);
        Debug.Log($"<color=cyan>Playing loop SFX with instance ID '{instanceId}'</color>");
    }

    private void AddLoopSfx(int instanceId, ActiveLoopSfx loopSfx)
    {
        _activeAudioSourcesLoops.Add(instanceId, loopSfx);
    }

    public void StopLoopSfx(int instanceId)
    {
        if (_activeAudioSourcesLoops.TryGetValue(instanceId, out var source))
        {
            _activeAudioSourcesLoops.Remove(instanceId);
            //_loopSfxList.Remove(source);
            _audioSourcePool.Release(source.AudioSource);
            Debug.Log($"<color=blue>stopped loop sfx with instance id {instanceId}</color>");
        }
        else
        {
            Debug.LogWarning($"No active Loop SFX found with instance ID '{instanceId}'.");
        }
    }

    private AudioSource GetAudiSourceFromPool(AudioClip clip, Vector3 position, float volume, float pitch, bool loop, AudioMixerGroup mixerGroup)
    {
        var source = _audioSourcePool.Get();
        source.transform.position = position;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
        source.outputAudioMixerGroup = mixerGroup;
        source.Play();
        return source;
    }

    public void SetLoopSfxPitch(int instanceId, float pitch)
    {
        if (_activeAudioSourcesLoops.TryGetValue(instanceId, out var source))
        {
            source.AudioSource.pitch = pitch;
        }
        else
        {
            Debug.LogWarning($"No active Loop SFX found with instance ID '{instanceId}'.");
        }
    }

    public void PlayMusic(SoundTrackId trackId)
    {
        var soundTrackData = _sfxDataContainer.GetSoundTrackData(trackId);
        _musicManager.PlayMusic(soundTrackData.GetAudioClip(), soundTrackData.Volume, true);
    }

    public void StopMusic()
    {
        _musicManager.StopMusic();
    }
}