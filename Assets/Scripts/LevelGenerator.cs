using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    public static Action<float> OnChangeSpeedAmount;
    [SerializeField] CameraController _cameraController;
    [SerializeField] int _startingChunksAmmount = 12;
    [SerializeField] Transform _chunkParentTransform;
    List<Chunk> _chunksList = new List<Chunk>();
    [SerializeField] LevelSettings _levelSettings;
    private Dictionary<string, ObjectPool<Chunk>> _chunkPools;
    private float _currentChunkMoveSpeed;    
    private void Start()
    {
        InitializeChunksPool();
        SpawnChunks();
    }

    private void InitializeChunksPool()
    {
        _currentChunkMoveSpeed = _levelSettings.InitialChunkSpeed;
        _chunkPools = new Dictionary<string, ObjectPool<Chunk>>();
        foreach (var prefab in _levelSettings.ChunkPrefab)
        {
            string chunkPrefabName = prefab.name;
            _chunkPools.Add(chunkPrefabName,
            new ObjectPool<Chunk>(
                createFunc: () =>
                {
                    Chunk newChunk = Instantiate(prefab, _chunkParentTransform);
                    newChunk.gameObject.name = prefab.name;
                    return newChunk;
                },
                actionOnGet: (chunk) => chunk.gameObject.SetActive(true),
                actionOnRelease: (chunk) => chunk.gameObject.SetActive(false),
                actionOnDestroy: (chunk) => Destroy(chunk.gameObject)
            ));
        }
    }

    private void OnEnable()
    {
        OnChangeSpeedAmount += ChangeChunkMoveSpeed;
    }

    private void OnDisable()
    {
        OnChangeSpeedAmount -= ChangeChunkMoveSpeed;
    }

    private void Update()
    {
        MoveChunks();
    }

    private void SpawnChunks()
    {
        for (int i = 0; i < _startingChunksAmmount; i++)
        {
            PlaceNewChunk();
        }
    }

    private void PlaceNewChunk()
    {
        Chunk newChunk = GetRandomChunkFromPool();
        newChunk.transform.position = CalculateSpawnPosition();
        newChunk.transform.rotation = Quaternion.identity;
        newChunk.transform.SetParent(_chunkParentTransform);
        _chunksList.Add(newChunk);
    }

    private Chunk GetRandomChunkFromPool()
    {
        // int poolIndex = UnityEngine.Random.Range(0, _chunkPools.Count);
        // return _chunkPools[poolIndex].Get();
        return _chunkPools.Values.ToList()[Random.Range(0,_chunkPools.Count)].Get();
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
            chunk.transform.Translate(Vector3.back * _currentChunkMoveSpeed * Time.deltaTime);

            if (chunk.transform.position.z <= Camera.main.transform.position.z - _levelSettings.ChunkLength)
            {
                _chunksList.Remove(chunk);
                ReleaseChunk(chunk);
                PlaceNewChunk();
            }
        }
    }

    private void ReleaseChunk(Chunk chunk)
    {
        string chunkName = chunk.name;
        _chunkPools[chunkName].Release(chunk);
    }

    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        _currentChunkMoveSpeed += speedAmount;
        _currentChunkMoveSpeed = Mathf.Clamp(_currentChunkMoveSpeed, _levelSettings.MinChunkMoveSpeed, _levelSettings.MaxChunkMoveSpeed);
        //TODO: modify gravity? use physics obstacles at all?
        //Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, Physics.gravity.z - speedAmount);
        _cameraController.ChangeCaeramFOV(speedAmount);
    }
}
