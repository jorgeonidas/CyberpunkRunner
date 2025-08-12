using UnityEngine;

public class DestructibleObstacle : MonoBehaviour, IDestroy
{
    VFXSpawner _vfxSpawner;
    PooledObject _poolObject;
    //test
    [SerializeField] Rigidbody _destroyectVisuals;
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
        Rigidbody rigidbody = Instantiate(_destroyectVisuals, transform.position, transform.rotation);
        Vector3 upForce = Vector3.up * Random.Range(5, 10);
        Vector3 backForce = Vector3.forward * Random.Range(-5, 5);
        Vector3 sideForce = Random.value < 0.5f ? Vector3.right * 10 : Vector3.left * 10;
        rigidbody.AddForce(upForce + backForce + sideForce, ForceMode.Impulse);
        // Torque aleatorio
        float torque = 5f;
        Vector3 randomTorque = new Vector3(
            Random.Range(-torque, torque),
            Random.Range(-torque, torque),
            Random.Range(-torque, torque)
        );
        rigidbody.AddTorque(randomTorque, ForceMode.Impulse);
        _vfxSpawner?.PlayParticleEffect(transform.position);
        _poolObject?.Release();
    }
}
