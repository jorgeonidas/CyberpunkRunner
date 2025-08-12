using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingObjectsSpawner : MonoBehaviour
{
    [SerializeField] LevelSettings _levelSettings;
    [SerializeField] string[] _vehicleObstaclesIds;
    [SerializeField] float _spawnZOffset = 5f;
    [SerializeField] int _maxCoinsToSpawn = 10;
    private List<float> _lanes = new List<float>();
    private List<int> _previousVehicleLanes = new List<int>(); 
    private List<int> _currentAvailableLanes = new List<int>();
    float _chunkLength;
    private float[] _horizontalLanes;
    private string[] _powerupsIds = { StringConstants.PowerupIds.SpeedBoost };
    private void Start()
    {
        Initialize();
    }

    private void OnEnable()
    {
        LevelGenerator.OnChunkPlaced += PlaceProps;
    }

    void OnDisable()
    {
        LevelGenerator.OnChunkPlaced -= PlaceProps;
    }

    public void Initialize()
    {
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
        int laesOcuppiedCoiunt = Random.Range(1, Mathf.Min(_lanes.Count, allowedLanes.Count + 1));
        for (int i = 0; i < laesOcuppiedCoiunt; i++)
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
        int coinsToSpawn = Random.Range(1, _maxCoinsToSpawn);
        float spacing = _chunkLength / _maxCoinsToSpawn;
        float startZ = transform.position.z + _spawnZOffset + _chunkLength / 2f;

        for (int i = 0; i < coinsToSpawn; i++)
        {
            float zPos = startZ - spacing * i;
            Vector3 spawnPos = new Vector3(xPosition, 0f, zPos);
            PoolManager.Instance.Get(StringConstants.Coin, spawnPos, transform.rotation);
        }
    }

    public void SpawnPowerup()
    {
        if (_currentAvailableLanes.Count == 0)
        {
            return;
        }
        int verticalLaneIndex = GetAvailableVerticalLane();
        int horizontalLaneIndex = Random.Range(0, _horizontalLanes.Length);
        float zCoordinate = _horizontalLanes[horizontalLaneIndex];
        Vector3 spawnPos = new Vector3(GetXPosition(verticalLaneIndex), 0f, transform.position.z + zCoordinate);
        //test
        PoolManager.Instance.Get(_powerupsIds.First(), spawnPos, transform.rotation);
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
}
