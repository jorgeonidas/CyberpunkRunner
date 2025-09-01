using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
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

    public List<int> GetOccupiedLanes() => _occupiedLanes;
}
