using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingObjectsSpawner : MonoBehaviour
{
    [SerializeField] LevelSettings _levelSettings;
    [SerializeField] MovingObject[] _vehiclePrefab;
    [SerializeField] MovingObject[] _coinPrefab;
    [SerializeField] float _spawnInterval = 3f;
    [SerializeField] float _objectsSpeed = 10f;
    [SerializeField] float _spawnZOffset = 5f;
    //coins
    [SerializeField] int _maxCoinsToSpawn = 10;
    //[SerializeField] float _coinLineLength = 10f; // equivalente al largo de un chunk visual
    //[SerializeField] float _coinSpawnChance = 0.6f;

    //private float _timer;
    private List<float> _lanes = new List<float>();
    private List<int> _previousVehicleLanes = new List<int>();  // evitar repetición
    private List<int> _currentAvailableLanes = new List<int>(); // para el ciclo actual
    private float _despawnZ;
    float _chunkLength;
    private void Start()
    {
        Initialize();
    }

    private void OnEnable() {
        LevelGenerator.OnChunkPlaced += PlaceProps;
    }

    void OnDisable()
    {
        LevelGenerator.OnChunkPlaced -= PlaceProps;
    }

    public void Initialize()
    {
        _lanes = _levelSettings.Lanes.ToList();
        //TODO: may be a Z value in the future
        _despawnZ = Camera.main.transform.position.z - (_levelSettings.ChunkLength * 2);
        // _timer = Random.Range(0f, _spawnInterval);
        _chunkLength = _levelSettings.ChunkLength;
    }

    private void PlaceProps()
    {
        PrepareLaneAvailability();               // reinicia lanes
        SpawnVehicles();                         // ocupa algunos
        SpawnCoins();
    }

    private void SpawnVehicles()
    {
        List<int> allowedLanes = _currentAvailableLanes
            .Except(_previousVehicleLanes)  // evita repetir
            .ToList();

        List<int> usedThisCycle = new List<int>();
        int vehicleCount = Random.Range(1, Mathf.Min(_lanes.Count, allowedLanes.Count + 1));

        for (int i = 0; i < vehicleCount; i++)
        {
            if (allowedLanes.Count == 0)
            {
                break;
            }

            int randomIndex = Random.Range(0, allowedLanes.Count);
            int laneIndex = allowedLanes[randomIndex];
            allowedLanes.RemoveAt(randomIndex);

            float xPos = transform.position.x + _lanes[laneIndex];
            float halfLength = _chunkLength / 2f;
            float zPos = transform.position.z + Random.Range(-halfLength, halfLength);

            MovingObject prefab = _vehiclePrefab[Random.Range(0, _vehiclePrefab.Length)];
            MovingObject vehicle = Instantiate(prefab, new Vector3(xPos, 0f, zPos), transform.rotation);
            vehicle.Initialize(_objectsSpeed, _despawnZ);

            _currentAvailableLanes.Remove(laneIndex); // marcar como ocupada
            usedThisCycle.Add(laneIndex);
        }

        _previousVehicleLanes = usedThisCycle; // guardar para el próximo ciclo
    }
    private void SpawnCoins()
    {
        if (_currentAvailableLanes.Count == 0)
        {
            return;
        }

        int laneIndex = _currentAvailableLanes[Random.Range(0, _currentAvailableLanes.Count)];
        _currentAvailableLanes.Remove(laneIndex); // ocupar la lane

        float xPosition = transform.position.x + _lanes[laneIndex];
        int coinsToSpawn = Random.Range(1, _maxCoinsToSpawn + 1);
        float spacing = _chunkLength / _maxCoinsToSpawn;
        float startZ = transform.position.z + _spawnZOffset + _chunkLength / 2f;

        for (int i = 0; i < coinsToSpawn; i++)
        {
            float zPos = startZ - spacing * i;
            Vector3 spawnPos = new Vector3(xPosition, 0f, zPos);

            MovingObject prefab = _coinPrefab[Random.Range(0, _coinPrefab.Length)];
            MovingObject coin = Instantiate(prefab, spawnPos, transform.rotation);
            coin.Initialize(_objectsSpeed, _despawnZ);
        }
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
