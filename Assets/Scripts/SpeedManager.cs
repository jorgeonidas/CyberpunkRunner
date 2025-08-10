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
    private float initialChunkSpeed;
    private float initialObjectsSpeed;
    private bool _stopped = false;
    private void Start()
    {
        initialChunkSpeed = _currentMovingChunkSpeed = _levelSettings.InitialChunkSpeed;
        initialObjectsSpeed = _currentMovingObjectsSpeed = _levelSettings.InitialObjectsSpeed;
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

    public void Stop()
    {
        _stopped = true;
        _currentMovingChunkSpeed = 0;
        _currentMovingObjectsSpeed = 0;//in the meantime I figure out a player dead animation
    }

    public void Restart()
    {
        _stopped = false;
        _currentMovingChunkSpeed = initialChunkSpeed;
        _currentMovingObjectsSpeed = initialObjectsSpeed;
    }

    public void AddSpeed(float speed)
    {
        if (_stopped)
        {
            return;
        }
        _currentMovingChunkSpeed += speed;
        _currentMovingObjectsSpeed += speed;
    }
}
