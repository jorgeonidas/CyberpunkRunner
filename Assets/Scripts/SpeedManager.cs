using System;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] LevelSettings _levelSettings;
    private float _currentMovingObjectsSpeed;
    private void Start()
    {
        _currentMovingObjectsSpeed = _levelSettings.InitialObjectsSpeed;
    }
    private void OnEnable()
    {
        MovingObject.OnAnyMovingObjectSpawned += InitializeMovingObject;
    }

    private void OnDisable()
    {
        MovingObject.OnAnyMovingObjectSpawned -= InitializeMovingObject;
    }

    private void InitializeMovingObject(MovingObject movingObject)
    {
        movingObject.Initialize(_currentMovingObjectsSpeed);
    }
}
