using UnityEngine;

public class VfxPoolable : PooledObject
{
    ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_particleSystem.isStopped)
        {
            Release();
        }
    }

    public override void OnGetFromPool()
    {
        _particleSystem.Play();
    }

    public void Stop()
    {
        _particleSystem.Stop();
    }
}
