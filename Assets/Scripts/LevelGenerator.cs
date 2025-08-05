using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] GameObject _chunkPrefab;
    [SerializeField] int _startingChunksAmmount = 12;
    [SerializeField] Transform _chunkParentTransform;
    [SerializeField] float _chunckLenth = 10;
    [SerializeField] float _chunkMoveSpeed = 10f;
    GameObject[] _chunksArray;
    private void Start()
    {
        _chunksArray = new GameObject[_startingChunksAmmount];
        SpawnChunks();
    }

    private void Update()
    {
        for (int i = 0; i < _chunksArray.Length; i++)
        {
            GameObject chunk = _chunksArray[i];
            chunk.transform.Translate(Vector3.back * _chunkMoveSpeed * Time.deltaTime);
        }
    }
    private void SpawnChunks()
    {
        for (int i = 0; i < _startingChunksAmmount; i++)
        {
            //We suppose the chunck parent transform is the starting position
            Vector3 newChunkPosition = CalculateSpawnPosition(i);
            GameObject newChunk = Instantiate(_chunkPrefab, newChunkPosition, Quaternion.identity, _chunkParentTransform);
            _chunksArray[i] = newChunk;
        }
    }

    private Vector3 CalculateSpawnPosition(int i)
    {
        return _chunkParentTransform.position + (Vector3.forward * i * _chunckLenth);
    }

    private void MoveChunks()
    {
        
    }
}
