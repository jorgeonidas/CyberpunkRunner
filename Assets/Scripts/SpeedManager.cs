using System;
using UnityEngine;

public class SpeedManager : MonoBehaviour
{
    public Action OnSpeedDifficultyIncreased;
    [SerializeField] SpeedSettings _speedSettings;
    [SerializeField] SpeedChangedEvent _speedChangedEvent;
    [Header("For testing, but theyre initialized from setting")]
    [SerializeField] private float _currentMovingObjectsSpeed;
    [SerializeField] private float _currentMovingChunkSpeed;
    public float CurrentChunksMoveSpeed => _currentMovingChunkSpeed;
    public float CurrentMovingObjectsSpeed => _currentMovingObjectsSpeed;
    public float MaxChunkSpeed => _maxChunkSpeed;
     public float NormalizedChunkSpeed => CurrentChunksMoveSpeed / MaxChunkSpeed;
    private float _initialChunkSpeed => _speedSettings.InitialChunkSpeed;
    private float _maxChunkSpeed => _speedSettings.MaxChunkSpeed;
    private float _initialObjectsSpeed => _speedSettings.InitialObjectsSpeed;
    private float _maxObjectsSpeed => _speedSettings.MaxObjectsSpeed;
    private float _targetChunkSpeed;
    private float _targetObjectsSpeed;
    private bool _stopped = false;
    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        _stopped = false;
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
        OnSpeedDifficultyIncreased += TryIncreaseSpeedDifficulty;
        MovingObject.OnAnyMovingObjectSpawned += InitializeMovingObject;
    }

    private void OnDisable()
    {
        OnSpeedDifficultyIncreased -= TryIncreaseSpeedDifficulty;   
        MovingObject.OnAnyMovingObjectSpawned -= InitializeMovingObject;
    }
    float _lastChunkSpeed = 0f;
    private void Update()
    {
        float newChunkSpeed = MoveTowardsTargetSpeed(_currentMovingChunkSpeed, _targetChunkSpeed);
        float newObjectsSpeed = MoveTowardsTargetSpeed(_currentMovingObjectsSpeed, _targetObjectsSpeed);

        // Detecta cambio significativo en la velocidad del chunk
        if (!Mathf.Approximately(newChunkSpeed, _lastChunkSpeed))
        {
            _speedChangedEvent?.Raise(newChunkSpeed); // Dispara el evento
            _lastChunkSpeed = newChunkSpeed;
        }

        _currentMovingChunkSpeed = newChunkSpeed;
        _currentMovingObjectsSpeed = newObjectsSpeed;
    }

    private float MoveTowardsTargetSpeed(float currentSpeed, float targetSpeed)
    {
        float acceleration = targetSpeed > currentSpeed ? _speedSettings.Acceleration : _speedSettings.Deceleration;
        float newSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
        return Mathf.Clamp(newSpeed, 0f, Mathf.Max(_maxChunkSpeed, _maxObjectsSpeed));
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

    public void TryIncreaseSpeedDifficulty(int chunksPassed)
    {
        if (chunksPassed % _speedSettings.ChunkSpeedIncreaseCycle == 0)
        {
            OnSpeedDifficultyIncreased?.Invoke();
        }
    }

    private void TryIncreaseSpeedDifficulty()
    {
        //game stopped
        if (_stopped)
        {
            return;
        }
        //reached the max difficulty speeds
        if (_currentMovingChunkSpeed >= _maxChunkSpeed && _currentMovingObjectsSpeed >= _maxObjectsSpeed)
        {
            return;
        }
        IncreaseSpeedClampled(_speedSettings.SpeedDifficultyIncrementPerCycle);
    }

    public float GetCurrentChunksMoveSpeedInKmH()
    {
        return _currentMovingChunkSpeed * 3.6f;
    }

    private void IncreaseSpeedClampled(float increase)
    {
        _targetChunkSpeed = Mathf.Clamp(_currentMovingChunkSpeed + increase, 0, _maxChunkSpeed);
        _targetObjectsSpeed = Mathf.Clamp(_currentMovingObjectsSpeed + increase, 0, _maxObjectsSpeed);
    }
}
