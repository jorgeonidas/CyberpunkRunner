using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] AnimationCurve _trayectoryCurve;
    [SerializeField] float _speed = 10f;
    [SerializeField] float _yAmplitude = 2.5f;
    Transform _target;
    float _totalDistance;
    public void Launch(Transform target)
    {
        _target = target;
        _totalDistance = Vector3.Distance(transform.position, _target.position);
    }

    private void Update()
    {
        if (_target == null)
        {
            return;
        }

        float currentDistance = Vector3.Distance(transform.position, _target.position);
        float t = currentDistance / _totalDistance;
        Vector3 nextPosition = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);
        nextPosition.y = _yAmplitude * _trayectoryCurve.Evaluate(t);
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
}
