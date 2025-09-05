using UnityEngine;

public class Rocket : PooledObject
{
    [SerializeField] AnimationCurve _trayectoryCurve;
    [SerializeField] float _speed = 10f;
    [SerializeField] float _yAmplitude = 2.5f;
    [SerializeField] VFXSpawner _trailVFXSpawner;
    Transform _target;
    float _totalDistance;
    Vector3 _startPosition;
    Vector3 _startTargetPosition;
    VfxPoolable _trailVFX;
    private Transform _targetGameObject;
    public void Launch(Transform target)
    {
        _target = target;
        if (_target == null || float.IsNaN(_target.position.x) || float.IsNaN(_target.position.y) || float.IsNaN(_target.position.z))
        {
            Debug.LogError("Invalid target transform for rocket.");
            return;
        }
        _startPosition = transform.position;
        _startTargetPosition = target.position;
        _totalDistance = Vector3.Distance(_startPosition, _startTargetPosition);
        if (SfxManager.Instance)
        {
            SfxManager.Instance.PlaySfx(SfxIdEnum.SfxId.RocketLaunch, transform.position);
        }
        _trailVFX = _trailVFXSpawner.PlayParticleEffect(transform.position) as VfxPoolable;

        // Guardar referencia al GameObject del target para chequeo de pooling
        _targetGameObject = target;
    }

    /*
 - The rocket moves from its start position to the target along a straight line in 3D space.
 - Each frame, we calculate the normalized direction vector from the start to the target.
 - The rocket advances incrementally by 'speed * deltaTime', ensuring smooth movement regardless of frame rate.
 - The total distance traveled is clamped so the rocket never overshoots the target.
 - The normalized progress 't' (from 0 to 1) represents how far the rocket is along its path.
 - The Y axis is modified by an animation curve, allowing for custom arc shapes (e.g., parabolic, sine wave).
 - At t=0, the rocket is at the start; at t=1, it reaches the target. The curve controls the vertical offset at each point.
*/
    private void Update()
    {

        if (_target == null || _targetGameObject == null || !_targetGameObject.gameObject.activeInHierarchy)
        {
            RocketImpact();
            return;
        }

        // Incremental movement to avoid overshooting the target
        Vector3 currentTargetPosition = _target.position;
        Vector3 direction = (currentTargetPosition - _startPosition).normalized;
        float moveStep = _speed * Time.deltaTime;
        float distanceFromStart = Vector3.Distance(_startPosition, transform.position);
        float nextDistance = Mathf.Min(distanceFromStart + moveStep, _totalDistance);
        float t = Mathf.Clamp01(nextDistance / _totalDistance);
        Vector3 nextPosition = _startPosition + direction * nextDistance;
        nextPosition.y = _yAmplitude * _trayectoryCurve.Evaluate(t);
        
        Vector3 moveDirection = nextPosition - transform.position;
        if (moveDirection.sqrMagnitude > 0.0001f)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
        }
        transform.position = nextPosition;
        if (_trailVFX)
        {
            _trailVFX.transform.position = nextPosition;
        }
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
    }

    private void RocketImpact()
    {
        ResetTrailVfx();
        Release();
    }

    private void ResetTrailVfx()
    {
        if (_trailVFX != null)
        {
            _trailVFX.Stop();
            _trailVFX = null;
        }
    }
}
