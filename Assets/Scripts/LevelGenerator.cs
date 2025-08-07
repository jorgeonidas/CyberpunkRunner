using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static Action<float> OnChangeSpeedAmount;
    [SerializeField] CameraController _cameraController;
    [SerializeField] int _startingChunksAmmount = 12;
    [SerializeField] Transform _chunkParentTransform;
    List<GameObject> _chunksList = new List<GameObject>();
    [SerializeField] LevelSettings _levelSettings;
    private float _currentChunkMoveSpeed;    
    private void Start()
    {
        _currentChunkMoveSpeed = _levelSettings.InitialChunkSpeed;
        SpawnChunks();
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
        Chunk newChunk = Instantiate(_levelSettings.ChunkPrefab, CalculateSpawnPosition(), Quaternion.identity, _chunkParentTransform);
        //initialize anything about chunk here
        _chunksList.Add(newChunk.gameObject);
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
            GameObject chunk = _chunksList[i];
            chunk.transform.Translate(Vector3.back * _currentChunkMoveSpeed * Time.deltaTime);

            if (chunk.transform.position.z <= Camera.main.transform.position.z - _levelSettings.ChunkLength)
            {
                _chunksList.Remove(chunk);
                Destroy(chunk);
                PlaceNewChunk();
            }
        }
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
