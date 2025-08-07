using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    //each 2.5f the lane
    [SerializeField] GameObject _fencePrefab;
    // [SerializeField] GameObject _applePrefab;
    // [SerializeField] GameObject _coinPrefab;
    [SerializeField] float[] _lanes = { -2.5f, 0f, 2.5f };
    [SerializeField] float _appleSpawnChance = 0.3f;
    [SerializeField] float _coinSpawnChance = 0.5f;
    List<int> availableLanes = new List<int>() { 0, 1, 2 };
    private const int _maxCoinsToSpawn = 5;
    private const float _chunckLength = 10f;

    private void Start()
    {
        //SpawnFences();
        // SpawnApple();
        // SpawnCoins();
    }

    private void SpawnFences()
    {
        // int fencesToSpawn = Random.Range(1, _lanes.Length);
        // for (int i = 0; i < fencesToSpawn; i++)
        // {
        //     if (availableLanes.Count <= 0)
        //     {
        //         break;
        //     }
        //     int selectedLane = SelectLane();
        //     float xPosition = transform.position.x + _lanes[selectedLane];
        //     Vector3 spawnPosition = new Vector3(xPosition, transform.position.y, transform.position.z);
        //     Instantiate(_fencePrefab, spawnPosition, Quaternion.identity, this.transform);
        // }
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

    private int SelectLane()
    {
        int randomLaneIndex = Random.Range(0, availableLanes.Count);
        int selectedLane = availableLanes[randomLaneIndex];
        availableLanes.RemoveAt(randomLaneIndex);
        return selectedLane;
    }
}
