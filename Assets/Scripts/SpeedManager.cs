using System;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    [SerializeField] SpeedSettings _speedSettings;
    [Header("For testing, but theyre initialized from setting")]
    [SerializeField] private float _currentMovingObjectsSpeed;
    [SerializeField] private float _currentMovingChunkSpeed;
    public float CurrentChunksMoveSpeed => _currentMovingChunkSpeed;
    public float CurrentMovingObjectsSpeed => _currentMovingObjectsSpeed;
    private float _initialChunkSpeed => _speedSettings.InitialChunkSpeed;
    private float _maxChunkSpeed => _speedSettings.MaxChunkSpeed;
    private float _initialObjectsSpeed => _speedSettings.InitialObjectsSpeed;
    private float _maxObjectsSpeed => _speedSettings.MaxObjectsSpeed;
    private float _targetChunkSpeed;
    private float _targetObjectsSpeed;
    private int _chunksPassed;
    private bool _stopped = false;
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _stopped = false;
        _chunksPassed = 0;
        InitializeChunksSpeeds();
        InitialieObjectsSpeeds();
    }

    private void InitializeChunksSpeeds()
    {
        //chunks
        _currentMovingChunkSpeed = 0;
        _targetChunkSpeed = _initialChunkSpeed;
    }

    private void InitialieObjectsSpeeds()
    {
        _currentMovingObjectsSpeed = 0;
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
            currentSpeed += _speedSettings.Acceleration * Time.deltaTime;
            if (currentSpeed > targetSpeed)
            {
                currentSpeed = targetSpeed;
            }
        }
        else
        {
            currentSpeed -= _speedSettings.Deceleration * Time.deltaTime;
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

    public void AddSpeedBonus(float speed)
    {
        if (_stopped)
        {
            return;
        }
        _targetChunkSpeed = _currentMovingChunkSpeed + speed;
        _targetObjectsSpeed = _currentMovingObjectsSpeed + speed;
    }

    public void TryIncreaseSpeedDifficulty()
    {
        _chunksPassed++;
        if (_chunksPassed % _speedSettings.ChunkSpeedIncreaseCycle == 0)
        {
            //reached the max difficulty speeds
            if (!_stopped && (_currentMovingChunkSpeed >= _maxChunkSpeed && _currentMovingObjectsSpeed >= _maxObjectsSpeed))
            {
                return;
            }
            IncreaseSpeedClampled(_speedSettings.SpeedDifficultyIncrementPerCycle);
        }
    }

    private void IncreaseSpeedClampled(float increase)
    {
        _targetChunkSpeed = Mathf.Clamp(_currentMovingChunkSpeed + increase, 0, _maxChunkSpeed);
        _targetObjectsSpeed = Mathf.Clamp(_currentMovingObjectsSpeed + increase, 0, _maxObjectsSpeed);
    }
}
