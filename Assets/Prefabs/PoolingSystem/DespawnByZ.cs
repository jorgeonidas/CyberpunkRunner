using UnityEngine;

[RequireComponent(typeof(PooledObject))]
public class DespawnByZ : MonoBehaviour
{
    [SerializeField] private float _despawnZ = -20f;
    private PooledObject _pooled;

    private void Awake() => _pooled = GetComponent<PooledObject>();

    private void Update()
    {
        if (transform.position.z < _despawnZ)
        {
            _pooled.Release();
        }
    }
}
