using UnityEngine;
using UnityEngine.Pool;

[DisallowMultipleComponent]
public class PooledObject : MonoBehaviour
{
    internal IObjectPool<PooledObject> Pool { get; set; }
    private bool _released;
    private string _poolObjectId;
    public string PoolObjectId => _poolObjectId;
    public bool Released => _released;

    public void SetPoolObjectId(string id) => _poolObjectId = id;
    public virtual void OnGetFromPool() => _released = false;
    public virtual void OnReleaseToPool(){}
    public void Release()
    {
        if (_released)
        {
            return;
        }
        _released = true;
        Pool?.Release(this);
    }
}