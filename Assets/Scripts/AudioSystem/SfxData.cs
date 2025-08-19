using UnityEngine;

[CreateAssetMenu(fileName = "SfxData", menuName = "Game SFX/SfxData")]
public class SfxData : ScriptableObject
{
    [Header("Audio Clips, place only one if its a loop SFX")]
    [SerializeField] private AudioClip[] _clips;
    [Range(0f, 1f)]
    [SerializeField] private float _volume = 1f;
    [Range(0f, 3f)]
    [SerializeField] private float _minPitch = 1f;
    [Range(0f, 3f)]
    [SerializeField] private float _maxPitch = 1f;

    public AudioClip[] Clips => _clips;
    public float Volume => _volume;
    public float MinPitch => _minPitch;
    public float MaxPitch => _maxPitch;

    public float GetRandomPitch()
    {
        return Random.Range(_minPitch, _maxPitch);
    }
    
    public AudioClip GetAudioClip()
    {
        if (_clips == null || _clips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned to SfxData.");
            return null;
        }
        return _clips[Random.Range(0, _clips.Length)];
    }
}
