using UnityEngine;
using UnityEngine.Pool;

[DisallowMultipleComponent]
public class PooledObject : MonoBehaviour
{
    internal IObjectPool<PooledObject> Pool { get; set; }
    private bool _released;
    public virtual void OnGetFromPool()  { _released = false; }
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