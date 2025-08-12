using UnityEngine;

public class DestructibleObstacle : MonoBehaviour, IDestroy
{
    VFXSpawner _vfxSpawner;
    PooledObject _poolObject;
    void Awake()
    {
        if (TryGetComponent<PooledObject>(out PooledObject poolObject))
        {
            _poolObject = poolObject;
        }
        if (TryGetComponent<VFXSpawner>(out VFXSpawner vfxSpawner))
        {
            _vfxSpawner = vfxSpawner;
        }
    }

    public void DestroyMe()
    {
        PoolManager.Instance?.Get(_poolObject.PoolObjectId + StringConstants.DestroyedSufix, transform.position, transform.rotation);
        _vfxSpawner?.PlayParticleEffect(transform.position);
        _poolObject?.Release();
    }
}
