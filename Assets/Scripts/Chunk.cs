using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] GameObject[] _obstaclesToSpawn;
    [SerializeField] float _appleSpawnChance = 0.3f;
    [SerializeField] float _coinSpawnChance = 0.5f;
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

    // private int SelectLaneIndex()
    // {
    //     int randomLaneIndex = Random.Range(0, _availableLanesIndexes.Count);
    //     int selectedLane = _availableLanesIndexes[randomLaneIndex];
    //     _availableLanesIndexes.RemoveAt(randomLaneIndex);
    //     return selectedLane;
    // }

    public List<int> GetOccupiedLanes() => _occupiedLanes;
}
