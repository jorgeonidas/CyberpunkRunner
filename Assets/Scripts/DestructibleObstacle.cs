using UnityEngine;

public class DestructibleObstacle : MonoBehaviour, IDestroy
{
    PooledObject _poolObject;
    void Awake()
    {
        if (TryGetComponent<PooledObject>(out PooledObject poolObject))
        {
            _poolObject = poolObject;
        }
    }
    public void DestroyMe()
    {
        if (_poolObject)
        {
            _poolObject.Release();
        }
    }
}
