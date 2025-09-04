using UnityEngine;

public abstract class PickUp : PooledObject
{
    private VFXSpawner _vfxSpawner;
    private ScreenShakeSource _screenShakeSource;
    private SfxEmitter _sfxEmmiter;
    private void Awake()
    {
        TryGetComponent(out _vfxSpawner);
        TryGetComponent(out _screenShakeSource);
        TryGetComponent(out _sfxEmmiter);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringConstants.PLAYER_TAG))
        {
            OnPickUp();
            _vfxSpawner?.PlayParticleEffect(other.transform.position);
            _screenShakeSource?.ShakeCamera();
            _sfxEmmiter?.PlaySfx();
            Release();
        }
    }
    protected abstract void OnPickUp();
}
