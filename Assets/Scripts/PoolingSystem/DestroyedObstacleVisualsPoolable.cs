using System.Collections;
using UnityEngine;

public class DestroyedObstacleVisualsPoolable : PooledObject
{
    Rigidbody _rigidbody;
    [SerializeField] float _upForceMin = 5f;
    [SerializeField] float _upForceMax = 10f;
    [SerializeField] float _backForce = 5f;
    [SerializeField] float _sideForce = 10f;
    [SerializeField] float _torque = 5f;
    [SerializeField] bool _selfRelease = true;
    [SerializeField] float _releaseDelay = 5f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnGetFromPool()
    {
        Vector3 upForce = Vector3.up * Random.Range(_upForceMin, _upForceMax);
        Vector3 backForce = Vector3.forward * Random.Range(-_backForce, _backForce);
        Vector3 sideForce = Random.value < 0.5f ? Vector3.right * _sideForce : Vector3.left * _sideForce;
        _rigidbody.AddForce(upForce + backForce + sideForce, ForceMode.Impulse);

        Vector3 randomTorque = new Vector3(
            Random.Range(-_torque, _torque),
            Random.Range(-_torque, _torque),
            Random.Range(-_torque, _torque)
        );
        _rigidbody.AddTorque(randomTorque, ForceMode.Impulse);
        if (_selfRelease)
        {
            StartCoroutine(ReleaseAfterDelay());
        }
    }

    IEnumerator ReleaseAfterDelay()
    {
        yield return new WaitForSeconds(_releaseDelay);
        Release();
    }

    public override void OnReleaseToPool()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}
