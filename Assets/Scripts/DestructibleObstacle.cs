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
        //play vfx spawn destroyed vehicle
        _vfxSpawner?.PlayParticleEffect(transform.position);
        _poolObject?.Release();
    }
}
