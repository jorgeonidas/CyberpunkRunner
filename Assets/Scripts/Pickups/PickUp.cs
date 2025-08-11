using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    private PooledObject _pooled;
    private VFXSpawner _vfxSpawner;
    private void Awake()
    {
        _pooled = GetComponent<PooledObject>();
        _vfxSpawner = GetComponent<VFXSpawner>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringConstants.PLAYER_TAG))
        {
            OnPickUp();
            if (_vfxSpawner)
            {
                _vfxSpawner.PlayParticleEffect(other.transform.position);
            }
            _pooled.Release();
        }
    }
    protected abstract void OnPickUp();
}
