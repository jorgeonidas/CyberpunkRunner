using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    //each 2.5f the lane
    [SerializeField] GameObject _obstacleToSpawn;
    // [SerializeField] GameObject _applePrefab;
    // [SerializeField] GameObject _coinPrefab;
    [SerializeField] float _appleSpawnChance = 0.3f;
    [SerializeField] float _coinSpawnChance = 0.5f;
    // private const int _maxCoinsToSpawn = 5;
    // private const float _chunckLength = 10f;
    private LevelGenerator _levelGenerator;
    private List<int> _availableLanesIndexes = new List<int>();
    float[] _lanesCoordinates;
    private List<GameObject> _spawnedObstacles = new List<GameObject>();
    private List<int> _occupiedLanes = new List<int>();
    public void Initialize(LevelGenerator levelGenerator, List<int> alreadyObstructedLanes)
    {
        ClearObstacles();
        _occupiedLanes.Clear();
        _levelGenerator = levelGenerator;
        _lanesCoordinates = _levelGenerator.GetLevelSettings().Lanes;
        InitializeAvailableLanesIndexes();
        RemoveAlreadyObstructedLanes(alreadyObstructedLanes);
        SpawnObstacles();
    }

    private void ClearObstacles()
    {
        foreach (GameObject obstacle in _spawnedObstacles)
        {
            Destroy(obstacle);
        }
    }

    private void RemoveAlreadyObstructedLanes(List<int> alreadyObstructedLanes)
    {
        foreach (var lane in alreadyObstructedLanes)
        {
            _availableLanesIndexes.Remove(lane);
        }
    }

    private void InitializeAvailableLanesIndexes()
    {
        _availableLanesIndexes.Clear();
        for (int i = 0; i < _lanesCoordinates.Length; i++)
        {
            _availableLanesIndexes.Add(i);
        }
    }

    // private void Start()
    // {

    //     // SpawnApple();
    //     // SpawnCoins();
    // }

    private void SpawnObstacles()
    {
        int fencesToSpawn = Random.Range(1, _lanesCoordinates.Length);
        for (int i = 0; i < fencesToSpawn; i++)
        {
            if (_availableLanesIndexes.Count <= 0)
            {
                break;
            }
            int selectedLane = SelectLaneIndex();
            float xPosition = transform.position.x + _lanesCoordinates[selectedLane];
            Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
            GameObject newObstacle = Instantiate(_obstacleToSpawn, spawnPosition, Quaternion.identity, this.transform);
            _spawnedObstacles.Add(newObstacle);
            _occupiedLanes.Add(selectedLane);
        }
    }
    private void SpawnApple()
    {
        // if (availableLanes.Count <= 0 || Random.value >= _appleSpawnChance)
        // {
        //     return;
        // }

        // int selectedLane = SelectLane();
        // float xPosition = transform.position.x + _lanes[selectedLane];
        // Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
        // Instantiate(_applePrefab, spawnPosition, Quaternion.identity, this.transform);
    }

    private void SpawnCoins()
    {
        // if (availableLanes.Count <= 0 || Random.value >= _coinSpawnChance)
        // {
        //     return;
        // }

        // //get lane
        // int selectedLane = SelectLane();
        // float xPosition = transform.position.x + _lanes[selectedLane];
        // float topHalfOffset = (_chunckLength / 2f);
        // //second value is exclusive
        // int coinsToSpawn = Random.Range(1, _maxCoinsToSpawn + 1);

        // //star from top half
        // float coinsSpacing = _chunckLength / (float)_maxCoinsToSpawn;
        // for (int i = 0; i < coinsToSpawn; i++)
        // {
        //     //from top to bottom
        //     float zPosition = transform.position.z + topHalfOffset - (coinsSpacing * i);
        //     Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, zPosition);
        //     Instantiate(_coinPrefab, spawnPosition, Quaternion.identity, this.transform);
        // }
    }

    private int SelectLaneIndex()
    {
        int randomLaneIndex = Random.Range(0, _availableLanesIndexes.Count);
        int selectedLane = _availableLanesIndexes[randomLaneIndex];
        _availableLanesIndexes.RemoveAt(randomLaneIndex);
        return selectedLane;
    }

    public List<int> GetOccupiedLanes() => _occupiedLanes;
}
