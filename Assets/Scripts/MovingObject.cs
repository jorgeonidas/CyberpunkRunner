using UnityEngine;

public class MovingObject : MonoBehaviour
{
    private float _speed;
    private float _despawnZ;

    public void Initialize(float speed, float despawnZ)
    {
        _speed = speed;
        _despawnZ = despawnZ;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);

        if (transform.position.z < _despawnZ)
        {
            Destroy(gameObject); // o ReturnToPool()
        }
    }
}
