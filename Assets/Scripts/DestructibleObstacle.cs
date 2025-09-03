using UnityEngine;

public class DestructibleObstacle : MonoBehaviour, IDestroy
{
    VFXSpawner _vfxSpawner;
    PooledObject _poolObject;
    SfxEmitter _sfxEmmiter;
    [SerializeField] OnPositionEvent _placeCoinEvent;

    void Awake()
    {
        TryGetComponent(out _poolObject);
        TryGetComponent(out _vfxSpawner);
        TryGetComponent(out _sfxEmmiter);
    }

    public void DestroyMe()
    {
        _placeCoinEvent?.Raise(transform.position);
        PoolManager.Instance?.Get(_poolObject.PoolObjectId + StringConstants.DestroyedSufix, transform.position, transform.rotation);
        _vfxSpawner?.PlayParticleEffect(transform.position);
        _sfxEmmiter?.PlaySfx();
        _poolObject?.Release();
    }
}
