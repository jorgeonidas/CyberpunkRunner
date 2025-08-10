using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    private PooledObject _pooled;
    private void Awake()
    {
        _pooled = GetComponent<PooledObject>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringConstants.PLAYER_TAG))
        {
            OnPickUp();
            _pooled.Release();
        }
    }
    protected abstract void OnPickUp();
}
