using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public static Action OnChunkPlaced;
    [SerializeField] int _startingChunksAmmount = 12;
    [SerializeField] Transform _chunkParentTransform;
    [SerializeField] LevelSettings _levelSettings;
    List<Chunk> _chunksList = new List<Chunk>();
    private Dictionary<string, ObjectPool<Chunk>> _chunkPools;
    private SpeedManager _speedManager;
    private int _chunksSurpassed = 0;
    private float _distanceTravelled = 0f;
    #region UnityLifeCycle    
    private void Start()
    {

    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    public void Initialize(SpeedManager speedManager)
    {
        _speedManager = speedManager;
        _chunksSurpassed = 0;
        _distanceTravelled = 0f;
        InitializeChunksPool();
        SpawnStartingChunks();
    }

    private void Update()
    {
        MoveChunks();
        UpdateDistanceTravelled();
    }
    #endregion

    private void SpawnStartingChunks()
    {
        for (int i = 0; i < _startingChunksAmmount; i++)
        {
            PlaceNewChunk();
        }
    }

    private void PlaceNewChunk()
    {
        Chunk newChunk = GetRandomChunkFromPool();
        InitializeNewChunk(newChunk);
        newChunk.transform.position = CalculateSpawnPosition();
        newChunk.transform.rotation = Quaternion.identity;
        newChunk.transform.SetParent(_chunkParentTransform);
        _chunksList.Add(newChunk);
    }

    private void InitializeNewChunk(Chunk newChunk)
    {
        List<int> preOccupiedLanes = new List<int>();
        if (_chunksList != null && _chunksList.Count() > 0)
        {
            preOccupiedLanes.AddRange(_chunksList.Last().GetOccupiedLanes());
        }
        newChunk.Initialize(this, preOccupiedLanes);
    }

    private Chunk GetRandomChunkFromPool()
    {
        return _chunkPools.Values.ToList()[Random.Range(0, _chunkPools.Count)].Get();
    }

    private Vector3 CalculateSpawnPosition()
    {
        Vector3 _chunkSpawnPosition = transform.position;
        if (_chunksList.Count == 0)
        {
            return _chunkSpawnPosition;
        }
        //take the last chunk, return the that position plus the chunk length in Z;
        Vector3 lasChunkPos = _chunksList[_chunksList.Count - 1].transform.position;
        _chunkSpawnPosition = lasChunkPos + (Vector3.forward * _levelSettings.ChunkLength);
        return _chunkSpawnPosition;
    }

    private void MoveChunks()
    {
        for (int i = 0; i < _chunksList.Count; i++)
        {
            Chunk chunk = _chunksList[i];
            chunk.transform.Translate(Vector3.back * _speedManager.CurrentChunksMoveSpeed * Time.deltaTime);

            if (chunk.transform.position.z <= Camera.main.transform.position.z - _levelSettings.ChunkLength)
            {
                _chunksSurpassed++;
                _chunksList.Remove(chunk);
                ReleaseChunk(chunk);
                PlaceNewChunk();
                OnChunkPlaced?.Invoke();
            }
        }
    }

    #region Pooling
    private void InitializeChunksPool()
    {
        _chunkPools = new Dictionary<string, ObjectPool<Chunk>>();
        foreach (var prefab in _levelSettings.ChunkPrefab)
        {
            string chunkPrefabName = prefab.name;
            _chunkPools.Add(chunkPrefabName,
            new ObjectPool<Chunk>(
                createFunc: () =>
                {
                    return InstantiateNewChunk(prefab);
                },
                actionOnGet: (chunk) => chunk.gameObject.SetActive(true),
                actionOnRelease: (chunk) => chunk.gameObject.SetActive(false),
                actionOnDestroy: (chunk) =>
                {
                    if (chunk)
                    {
                        Destroy(chunk.gameObject);
                    }
                }
            ));
        }
    }

    private Chunk InstantiateNewChunk(Chunk prefab)
    {
        Chunk newChunk = Instantiate(prefab, _chunkParentTransform);
        newChunk.gameObject.name = prefab.name;
        return newChunk;
    }

    private void ReleaseChunk(Chunk chunk)
    {
        string chunkId = chunk.name;
        _chunkPools[chunkId].Release(chunk);
        _speedManager.TryIncreaseSpeedDifficulty(_chunksSurpassed);
    }
    private void UpdateDistanceTravelled()
    {
       _distanceTravelled += _speedManager.CurrentChunksMoveSpeed * Time.deltaTime;
    }
    #endregion
    public float GetDistanceTravelled() => _distanceTravelled;
    public LevelSettings GetLevelSettings() => _levelSettings;
}
