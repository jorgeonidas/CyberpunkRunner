using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingObjectsSpawner : MonoBehaviour
{
    [SerializeField] GameStateChangedEvent _gameStateChangedEvent;
    [SerializeField] LevelSettings _levelSettings;
    [SerializeField] SpawnPowerupSettings _powerUpSetting;
    [SerializeField] string[] _vehicleObstaclesIds;
    [SerializeField] int _minCoinsToSpawn = 3;
    [SerializeField] int _maxCoinsToSpawn = 10;
    [Header("Spawn Settings")]
    [SerializeField] float _spawnInterval = 1f;
    [SerializeField] float _minSpawnSpeed = 0.7f;
    [SerializeField] float _spawnIntervalDecreaseRate = 0.05f;
    private List<float> _lanes = new List<float>();
    private List<int> _previousVehicleLanes = new List<int>();
    private List<int> _currentAvailableLanes = new List<int>();
    float _chunkLength;
    private float[] _horizontalLanes;
    private float _nextSpawnTime;
    private bool _stopped;
    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        _gameStateChangedEvent.OnEventRaised += OnGameStateChanged;
    }

    void OnDisable()
    {
        _gameStateChangedEvent.OnEventRaised -= OnGameStateChanged;
    }

    private void Update()
    {
        if (_stopped)
        {
            return;
        }

        if (Time.time >= _nextSpawnTime)
        {
            PlaceProps();
            _nextSpawnTime = Time.time + _spawnInterval;
        }
    }

    private void Initialize()
    {
        _stopped = false;
        _nextSpawnTime = Time.time + _spawnInterval;
        _lanes = _levelSettings.Lanes.ToList();
        _horizontalLanes = _levelSettings.HorizontalLanes;
        _chunkLength = _levelSettings.ChunkLength;
    }

    private void PlaceProps()
    {
        PrepareLaneAvailability();
        SpawnVehicles();
        SpawnCoins();
        SpawnPowerup();
    }

    private void SpawnVehicles()
    {
        List<float> availableHorizontalLanes = new List<float>();
        availableHorizontalLanes.AddRange(_horizontalLanes);

        List<int> allowedLanes = _currentAvailableLanes.Except(_previousVehicleLanes).ToList();
        List<int> obstructedThisCycle = new List<int>();
        int laesOcuppiedCount = Random.Range(1, Mathf.Min(_lanes.Count, allowedLanes.Count + 1));
        for (int i = 0; i < laesOcuppiedCount; i++)
        {
            if (allowedLanes.Count == 0)
            {
                break;
            }

            int randomVerticalLaneIndex = Random.Range(0, allowedLanes.Count);
            int laneIndex = allowedLanes[randomVerticalLaneIndex];
            allowedLanes.RemoveAt(randomVerticalLaneIndex);

            float xPos = GetXPosition(laneIndex);

            int horizontalLaneIndex = Random.Range(0, availableHorizontalLanes.Count);
            float zCoordinate = availableHorizontalLanes[horizontalLaneIndex];
            availableHorizontalLanes.RemoveAt(horizontalLaneIndex);
            float zPos = transform.position.z + zCoordinate;

            string vehicleId = _vehicleObstaclesIds[Random.Range(0, _vehicleObstaclesIds.Length)];
            PoolManager.Instance.Get(vehicleId, new Vector3(xPos, 0f, zPos), transform.rotation);
            _currentAvailableLanes.Remove(laneIndex);
            obstructedThisCycle.Add(laneIndex);
        }

        _previousVehicleLanes = obstructedThisCycle;
    }
    private void SpawnCoins()
    {
        if (_currentAvailableLanes.Count == 0)
        {
            return;
        }

        int verticalLalenIndex = GetAvailableVerticalLane();
        float xPosition = GetXPosition(verticalLalenIndex);
        int coinsToSpawn = Random.Range(_minCoinsToSpawn, _maxCoinsToSpawn);
        float spacing = _chunkLength / _maxCoinsToSpawn;
        float startZ = transform.position.z + _chunkLength / 2f;

        for (int i = 0; i < coinsToSpawn; i++)
        {
            float zPos = startZ - spacing * i;
            Vector3 spawnPos = new Vector3(xPosition, 0f, zPos);
            SpawnCoin(spawnPos);
        }
    }

    private void SpawnPowerup()
    {
        Debug.Log($"_currentAvailableLanes.Count = {_currentAvailableLanes.Count}");
        if (_currentAvailableLanes.Count == 0)
        {
            return;
        }
        int verticalLaneIndex = GetAvailableVerticalLane();
        int horizontalLaneIndex = Random.Range(0, _horizontalLanes.Length);
        float zCoordinate = _horizontalLanes[horizontalLaneIndex];
        Vector3 spawnPos = new Vector3(GetXPosition(verticalLaneIndex), 0f, transform.position.z + zCoordinate);
        var chosenPowerup = _powerUpSetting.ChoosePowerup();
        Debug.Log($"Chosen powerup: {chosenPowerup}");
        if (chosenPowerup != null && chosenPowerup.Id != StringConstants.PowerupIds.None)
        {
            PoolManager.Instance.Get(chosenPowerup.Id, spawnPos, transform.rotation);
        }

    }

    private float GetXPosition(int verticalLalenIndex)
    {
        return transform.position.x + _lanes[verticalLalenIndex];
    }

    private int GetAvailableVerticalLane()
    {
        int laneIndex = _currentAvailableLanes[Random.Range(0, _currentAvailableLanes.Count)];
        _currentAvailableLanes.Remove(laneIndex);
        return laneIndex;
    }

    private void PrepareLaneAvailability()
    {
        _currentAvailableLanes.Clear();
        for (int i = 0; i < _lanes.Count; i++)
        {
            _currentAvailableLanes.Add(i);
        }
    }

    private void OnGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                Resume();
                break;
            case GameState.Paused:
                Pause();
                break;
            case GameState.GameOver:
                Pause();
                break;
        }
    }

    private void Pause()
    {
        _stopped = true;
    }

    private void Resume()
    {
        _stopped = false;
        _nextSpawnTime = Time.time + _spawnInterval;
    }

    public void TryDecreaseSpawnrate()
    {
        if (_spawnInterval > _minSpawnSpeed)
        {
            _spawnInterval = Mathf.Max(_minSpawnSpeed, _spawnInterval - _spawnIntervalDecreaseRate);
        }
    }

    public void SpawnCoin(Vector3 spawnPos)
    {
        PoolManager.Instance.Get(StringConstants.Coin, spawnPos, transform.rotation);
    }
}
