using System;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public static Action<float> OnChangeSpeedAmount;
    [SerializeField] CameraController _cameraController;
    [SerializeField] GameObject _chunkPrefab;
    [SerializeField] int _startingChunksAmmount = 12;
    [SerializeField] Transform _chunkParentTransform;
    [SerializeField] float _chunckLength = 10;
    [SerializeField] float _chunkMoveSpeed = 10f;
    [SerializeField] float _minChunkMoveSpeed = 2f;
    [SerializeField] float _maxChunkMoveSpeed = 15f;
    List<GameObject> _chunksList = new List<GameObject>();

    private void Start()
    {
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
        GameObject newChunk = Instantiate(_chunkPrefab, CalculateSpawnPosition(), Quaternion.identity, _chunkParentTransform);
        _chunksList.Add(newChunk);
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
        _chunkSpawnPosition = lasChunkPos + (Vector3.forward * _chunckLength);
        return _chunkSpawnPosition;
    }

    private void MoveChunks()
    {
        for (int i = 0; i < _chunksList.Count; i++)
        {
            GameObject chunk = _chunksList[i];
            chunk.transform.Translate(Vector3.back * _chunkMoveSpeed * Time.deltaTime);

            if (chunk.transform.position.z <= Camera.main.transform.position.z - _chunckLength)
            {
                _chunksList.Remove(chunk);
                Destroy(chunk);
                PlaceNewChunk();
            }
        }
    }

    public void ChangeChunkMoveSpeed(float speedAmount)
    {
        _chunkMoveSpeed += speedAmount;
        _chunkMoveSpeed = Mathf.Clamp(_chunkMoveSpeed, _minChunkMoveSpeed, _maxChunkMoveSpeed);
        //Physics.gravity = new Vector3(Physics.gravity.x, Physics.gravity.y, Physics.gravity.z - speedAmount);
        _cameraController.ChangeCaeramFOV(speedAmount);
    }
}
