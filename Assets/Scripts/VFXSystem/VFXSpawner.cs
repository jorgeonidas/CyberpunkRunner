using UnityEngine;

public class VFXSpawner : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] float _yOffset = 0.5f;
    ParticleSystem _particleSystemInstance;

    private void Awake()
    {
        _particleSystemInstance = Instantiate(_particleSystem, transform.position, Quaternion.identity);
    }

    public void PlayParticleEffect(Vector3 position)
    {
        _particleSystemInstance.transform.position = position + Vector3.up *_yOffset;
        _particleSystemInstance.Play();
    }
}
