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
    [SerializeField] int _maxCoinsToSpawn = 5;
    [SerializeField] float _coinLineLength = 10f; // equivalente al largo de un chunk visual
    [SerializeField] float _coinSpawnChance = 0.6f;

    private float _timer;
    private List<float> _lanes = new List<float>();
    List<float> _availableLanes = new List<float>();
    private float _despawnZ;
    int _prevRandomLaneIndex = -1;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        _lanes = _levelSettings.Lanes.ToList();
        //TODO: may be a Z value in the future
        _despawnZ = Camera.main.transform.position.z - (_levelSettings.ChunkLength * 2);
        _timer = Random.Range(0f, _spawnInterval);
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            SpawnVehicle();
            SpawnCoins();
            _timer = _spawnInterval;
        }
    }

    private void SpawnVehicle()
    {
        int randomLaneIndex = SelecRandomLaneIndex();
        _prevRandomLaneIndex = randomLaneIndex;
        float xPos = transform.position.x + _lanes[randomLaneIndex];
        float zPos = transform.position.z + _spawnZOffset;
        MovingObject prefab = _vehiclePrefab[Random.Range(0, _vehiclePrefab.Length)];
        MovingObject vehicle = Instantiate(prefab, new Vector3(xPos, 0f, zPos), transform.rotation);
        vehicle.Initialize(_objectsSpeed, _despawnZ);
    }

    private int SelecRandomLaneIndex()
    {
        _availableLanes.Clear();
        _availableLanes.AddRange(_lanes);
        if (_prevRandomLaneIndex != -1)
        {
            float laneToRemove = _lanes[_prevRandomLaneIndex];
            _availableLanes.Remove(laneToRemove);
        }
        int randomLaneInAvailable = Random.Range(0, _availableLanes.Count);
        int laneIndex = _lanes.IndexOf(_availableLanes[randomLaneInAvailable]);
        return laneIndex;
    }

    private void SpawnCoins()
    {
        if (/*Random.value >= _coinSpawnChance ||*/ _lanes.Count == 0)
        {
            return;
        }

        // Elegir lane diferente al del vehículo
        List<int> availableIndexes = Enumerable.Range(0, _lanes.Count).ToList();
        if (_prevRandomLaneIndex != -1)
        {
            availableIndexes.Remove(_prevRandomLaneIndex);
        }

        if (availableIndexes.Count == 0)
        {
            return;
        }

        int selectedLaneIndex = availableIndexes[Random.Range(0, availableIndexes.Count)];
        float xPosition = transform.position.x + _lanes[selectedLaneIndex];

        // Determinar cantidad y espaciado
        int coinsToSpawn = Random.Range(1, _maxCoinsToSpawn + 1);
        float spacing = _coinLineLength / _maxCoinsToSpawn;
        float startZ = transform.position.z + _spawnZOffset + _coinLineLength / 2f;
        //refactor to put any prefab?s
        for (int i = 0; i < coinsToSpawn; i++)
        {
            float zPosition = startZ - spacing * i;
            Vector3 spawnPosition = new Vector3(xPosition, 0f, zPosition);

            MovingObject prefab = _coinPrefab[Random.Range(0, _coinPrefab.Length)];
            MovingObject coin = Instantiate(prefab, spawnPosition, transform.rotation);
            coin.Initialize(_objectsSpeed, _despawnZ);
        }
    }
}
