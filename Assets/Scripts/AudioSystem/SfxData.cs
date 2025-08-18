using UnityEngine;

[CreateAssetMenu(fileName = "SfxData", menuName = "Game SFX/SfxData")]
public class SfxData : ScriptableObject
{
    [SerializeField] private AudioClip _clip;
    [Range(0f, 1f)]
    [SerializeField] private float _volume = 1f;
    [Range(0f, 3f)]
    [SerializeField] private float _minPitch = 1f;
    [Range(0f, 3f)]
    [SerializeField] private float _maxPitch = 1f;

    public AudioClip Clip => _clip;
    public float Volume => _volume;
    public float MinPitch => _minPitch;
    public float MaxPitch => _maxPitch;
    
    public float GetRandomPitch()
    {
        return Random.Range(_minPitch, _maxPitch);
    }
}
