using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Sci-FiRunner/LevelSettings")]
public class LevelSettings : ScriptableObject
{
    [Header("Chunk Dimmensions")]
    [SerializeField] Chunk _chunkPrefab;
    [SerializeField] float _chunkLength = 10;
    [SerializeField] float[] _lanes = { -3f, 0f, 3f };
    [Header("Level Speeds")]
    [SerializeField] float _initialChunkSpeed = 10;
    [SerializeField] float _minChunkMoveSpeed = 2f;
    [SerializeField] float _maxChunkMoveSpeed = 15f;

    public Chunk ChunkPrefab => _chunkPrefab;
    public float ChunkLength => _chunkLength;
    public float[] Lanes => _lanes;

    public float InitialChunkSpeed => _initialChunkSpeed;
    public float MinChunkMoveSpeed => _minChunkMoveSpeed;
    public float MaxChunkMoveSpeed => _maxChunkMoveSpeed;
}
