using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VehiclesSpawner : MonoBehaviour
{
    [SerializeField] LevelSettings _levelSettings;
    [SerializeField] MovingObject[] _vehiclePrefab;
    [SerializeField] float _spawnInterval = 3f;
    [SerializeField] float _vehicleSpeed = 10f;
    [SerializeField] float _spawnZOffset = 5f;

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
        _despawnZ = Camera.main.transform.position.z - _levelSettings.ChunkLength;
        _timer = Random.Range(0f, _spawnInterval);
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer <= 0f)
        {
            SpawnVehicle();
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
        MovingObject vehicle = Instantiate(prefab, new Vector3(xPos, 0f, zPos), Quaternion.Euler(0f, 180f, 0f));
        vehicle.Initialize(_vehicleSpeed, _despawnZ);
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
}
