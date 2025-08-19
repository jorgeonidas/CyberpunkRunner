using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    private PooledObject _pooled;
    private VFXSpawner _vfxSpawner;
    private ScreenShakeSource _screenShakeSource;
    private SfxEmitter _sfxEmmiter;
    private void Awake()
    {
        TryGetComponent(out _pooled);
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
            _pooled?.Release();
        }
    }
    protected abstract void OnPickUp();
}
