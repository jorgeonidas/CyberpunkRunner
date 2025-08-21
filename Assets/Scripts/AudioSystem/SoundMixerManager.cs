using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    private const int MinDb = -70;
    private const int MaxDb = 0;

    public void SetSfxVolume(float level)
    {
        _audioMixer.SetFloat("SFXVolume", LevelToDecibels(level));
    }

    public void SetMusicVolime(float level)
    {
        _audioMixer.SetFloat("MusicVolume", LevelToDecibels(level));
    }

    private static float LevelToDecibels(float level)
    {
        float minLevel = 0.0001f;
        return Mathf.Log10(Mathf.Clamp(level, minLevel, 1f)) * 20f;
    }
}
