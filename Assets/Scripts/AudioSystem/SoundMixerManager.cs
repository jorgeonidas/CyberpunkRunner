using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer _audioMixer;
    const float MuteDb = -80f; // o el mínimo que tengas expuesto en el mixer
    public void SetSfxVolume(float level)
    {
        _audioMixer.SetFloat("SFXVolume", LinearToDecibels(level));
    }

    public void SetMusicVolime(float level)
    {
        _audioMixer.SetFloat("MusicVolume", LinearToDecibels(level));
    }

    private static float LinearToDecibels(float linear)
    {
        linear = Mathf.Clamp01(linear);
        if (linear <= 0.0001f) return MuteDb;          // evita -Inf dB
        return 20f * Mathf.Log10(linear);              // 1 -> 0 dB, 0.5 -> ~-6 dB, 0.25 -> ~-12 dB
    }
}
