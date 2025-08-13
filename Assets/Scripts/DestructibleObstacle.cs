using UnityEngine;

public class DestructibleObstacle : MonoBehaviour, IDestroy
{
    VFXSpawner _vfxSpawner;
    PooledObject _poolObject;


    void Awake()
    {
        TryGetComponent(out _poolObject);
        TryGetComponent(out _vfxSpawner);
    }

    public void DestroyMe()
    {
        PoolManager.Instance?.Get(_poolObject.PoolObjectId + StringConstants.DestroyedSufix, transform.position, transform.rotation);
        _vfxSpawner?.PlayParticleEffect(transform.position);
        _poolObject?.Release();
    }
}
