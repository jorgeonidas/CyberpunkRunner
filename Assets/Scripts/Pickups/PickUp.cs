using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    private PooledObject _pooled;
    private VFXSpawner _vfxSpawner;
    private ScreenShakeSource _screenShakeSource;
    private void Awake()
    {
        TryGetComponent(out _pooled);
        TryGetComponent(out _vfxSpawner);
        TryGetComponent(out _screenShakeSource);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringConstants.PLAYER_TAG))
        {
            OnPickUp();
            _vfxSpawner?.PlayParticleEffect(other.transform.position);
            _screenShakeSource?.ShakeCamera();
            _pooled?.Release();
        }
    }
    protected abstract void OnPickUp();
}
