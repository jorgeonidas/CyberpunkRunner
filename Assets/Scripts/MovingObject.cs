using System;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public static Action<MovingObject> OnAnyMovingObjectSpawned;
    private float _speed;

    private void OnEnable()
    {
        OnAnyMovingObjectSpawned?.Invoke(this);
    }

    private void OnDisable()
    {
        OnAnyMovingObjectSpawned?.Invoke(this);
    }

    public void Initialize(float speed)
    {
        _speed = speed;
    }


    private void Update()
    {
        transform.Translate(Vector3.forward * _speed * Time.deltaTime);
    }
}
