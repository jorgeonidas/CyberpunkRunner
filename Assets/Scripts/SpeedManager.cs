using System;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] LevelSettings _levelSettings;
    [Header("For testing, but theyre initialized from setting")]
    [SerializeField] private float _currentMovingObjectsSpeed;
    [SerializeField] private float _currentMovingChunkSpeed;
    public float CurrentChunksMoveSpeed => _currentMovingChunkSpeed;
    public float CurrentMovingObjectsSpeed => _currentMovingObjectsSpeed;
    private void Start()
    {
        _currentMovingChunkSpeed = _levelSettings.InitialChunkSpeed;
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
        movingObject.Initialize(this);
    }
}
