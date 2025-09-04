using UnityEngine;

public class Rocket : PooledObject
{
    [SerializeField] AnimationCurve _trayectoryCurve;
    [SerializeField] float _speed = 10f;
    [SerializeField] float _yAmplitude = 2.5f;
    Transform _target;
    float _totalDistance;
    Vector3 _startPosition;
    Vector3 _startTargetPosition;
    float _launchTime;
    float _duration;
    public void Launch(Transform target)
    {
        _target = target;
        _startPosition = transform.position;
        _startTargetPosition = target.position;
        _totalDistance = Vector3.Distance(_startPosition, _startTargetPosition);
        _duration = _totalDistance / _speed;
        _launchTime = Time.time;
        if (SfxManager.Instance)
        {
            SfxManager.Instance.PlaySfx(SfxIdEnum.SfxId.RocketLaunch, transform.position);
        }
    }

    private void Update()
    {
        if (_target == null)
        {
            return;
        }

        float elapsed = Time.time - _launchTime;
        float t = Mathf.Clamp01(elapsed / _duration);
        Vector3 currentTargetPosition = _target.position;
        Vector3 direction = (currentTargetPosition - _startPosition).normalized;
        float distanceToTravel = _totalDistance * t;
        Vector3 nextPosition = _startPosition + direction * distanceToTravel;
        nextPosition.y = _yAmplitude * _trayectoryCurve.Evaluate(t);

        // Calcular dirección de movimiento antes de actualizar la posición
        Vector3 moveDirection = nextPosition - transform.position;
        if (moveDirection.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        transform.position = nextPosition;
    }

    void OnDrawGizmos()
    {
        if (_target != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _target.position);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEnter {other.name}");
        if (other.TryGetComponent(out IDestroy destructible))
        {
            Debug.Log("here!");
            destructible.DestroyMe();
            RocketImpact();
        }
        //instantiate explosion
        //destroy this (add to pool manager)
    }

    private void RocketImpact()
    {
        Release();
    }
}
