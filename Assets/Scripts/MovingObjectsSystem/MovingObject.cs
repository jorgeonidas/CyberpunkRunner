using System;
using UnityEngine;

public class MovingObject : MonoBehaviour
{
    protected SpeedManager _speedManager;
    public static Action<MovingObject> OnAnyMovingObjectSpawned;

    private void OnEnable()
    {
        OnAnyMovingObjectSpawned?.Invoke(this);
    }

    public void Initialize(SpeedManager speedManager)
    {
        _speedManager = speedManager;
    }


    protected void Update()
    {
        transform.Translate(Vector3.forward * _speedManager.CurrentMovingObjectsSpeed * Time.deltaTime);
    }
}
