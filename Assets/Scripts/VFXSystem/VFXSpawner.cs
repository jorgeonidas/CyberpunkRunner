using UnityEngine;

public class VFXSpawner : MonoBehaviour
{
    [SerializeField] string _poolId;
    [SerializeField] float _yOffset = 0.5f;

    public void PlayParticleEffect(Vector3 position)
    {
        PoolManager.Instance?.Get(_poolId, position + Vector3.up * _yOffset, transform.rotation);
    }
}
