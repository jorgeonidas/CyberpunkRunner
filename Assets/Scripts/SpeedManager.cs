using System;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] LevelSettings _levelSettings;
    [Header("For testing, but theyre initialized from setting")]
    [SerializeField] private float _currentMovingObjectsSpeed;
    [SerializeField] private float _currentMovingChunkSpeed;
    [Header("speed change acceleration/deceleration")]
    [SerializeField] private float _acceleration = 14f;
    [SerializeField] private float _deceleration = 20f;
    public float CurrentChunksMoveSpeed => _currentMovingChunkSpeed;
    public float CurrentMovingObjectsSpeed => _currentMovingObjectsSpeed;
    private float _initialChunkSpeed;
    private float _initialObjectsSpeed;
    private float _targetChunkSpeed;
    private float _targetObjectsSpeed;
    private bool _stopped = false;
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        InitializeChunksSpeeds();
        InitialieObjectsSpeeds();
    }

    private void InitializeChunksSpeeds()
    {
        //chunks
        _currentMovingChunkSpeed = 0;
        _initialChunkSpeed = _levelSettings.InitialChunkSpeed;
        _targetChunkSpeed = _initialChunkSpeed;
    }

    private void InitialieObjectsSpeeds()
    {
        _currentMovingObjectsSpeed = 0;
        _initialObjectsSpeed = _levelSettings.InitialObjectsSpeed;
        _targetObjectsSpeed = _initialObjectsSpeed;
    }

    private void OnEnable()
    {
        MovingObject.OnAnyMovingObjectSpawned += InitializeMovingObject;
    }

    private void OnDisable()
    {
        MovingObject.OnAnyMovingObjectSpawned -= InitializeMovingObject;
    }

    private void Update()
    {

        _currentMovingChunkSpeed = MoveTowarsTargetSpeed(_currentMovingChunkSpeed, _targetChunkSpeed);
        _currentMovingObjectsSpeed = MoveTowarsTargetSpeed(_currentMovingObjectsSpeed, _targetObjectsSpeed);
    }

    private float MoveTowarsTargetSpeed(float currentSpeed, float targetSpeed)
    {
        if (targetSpeed > currentSpeed)
        {
            currentSpeed += _acceleration * Time.deltaTime;
            if (currentSpeed > targetSpeed)
            {
                currentSpeed = targetSpeed;
            }
        }
        else
        {
            currentSpeed -= _deceleration * Time.deltaTime;
            if (currentSpeed < targetSpeed)
            {
                currentSpeed = targetSpeed;
            }
        }

        return Mathf.Max(0f, currentSpeed);
    }

    private void InitializeMovingObject(MovingObject movingObject)
    {
        movingObject.Initialize(this);
    }

    public void Stop(bool gameOver = false)
    {
        _stopped = true;
        _targetChunkSpeed = 0;
        if (!gameOver)
        {
            _targetObjectsSpeed = 0;
        }
    }

    public void Restart()
    {
        _stopped = false;
        Initialize();
    }

    public void AddSpeed(float speed)
    {
        if (_stopped)
        {
            return;
        }
        _targetChunkSpeed = _currentMovingChunkSpeed + speed;
        _targetObjectsSpeed = _currentMovingObjectsSpeed + speed;
    }
}
