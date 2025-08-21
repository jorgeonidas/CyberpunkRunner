using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] AudioSource _audioSource;

    public void PlayMusic(AudioClip clip, float volume, bool loop = true)
    {
        if (_audioSource.clip == clip && _audioSource.isPlaying)
        {
            return;
        }

        _audioSource.clip = clip;
        _audioSource.loop = loop;
        _audioSource.volume = volume;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
