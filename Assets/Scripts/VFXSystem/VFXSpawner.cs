using UnityEngine;

public class VFXSpawner : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    ParticleSystem _particleSystemInstance;

    private void Awake()
    {
        _particleSystemInstance = Instantiate(_particleSystem, transform.position, Quaternion.identity);
    }

    public void PlayParticleEffect(Vector3 position)
    {
        _particleSystemInstance.transform.position = position;
        _particleSystemInstance.Play();
    }
}
