using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }
    [SerializeField] private SfxDataContainer sfxDataContainer;
    [SerializeField] private int poolSize = 10;

    private ObjectPool<AudioSource> audioSourcePool;

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
        if(source != null)
        {
            Destroy(source.gameObject);
        }
    }

    private void PlaySfx(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        var source = audioSourcePool.Get();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.Play();
        StartCoroutine(ReleaseWhenDone(source));
    }

    private IEnumerator ReleaseWhenDone(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        audioSourcePool.Release(source);
    }
    
    public void PlaySfx(SfxIdEnum.SfxId sfxId)
    {
        var sfxData = sfxDataContainer.GetSfxData(sfxId);
        if (sfxData != null && sfxData.Clip != null)
        {
            PlaySfx(sfxData.Clip, sfxData.Volume, sfxData.GetRandomPitch());
        }
        else
        {
            Debug.LogWarning($"SFX with ID '{sfxId}' not found or has no clip assigned.");
        }
    }
}