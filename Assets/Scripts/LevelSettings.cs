using UnityEngine;

[CreateAssetMenu(fileName = "LevelSettings", menuName = "Sci-FiRunner/LevelSettings")]
public class LevelSettings : ScriptableObject
{
    [Header("Chunk Dimmensions")]
    [SerializeField] Chunk[] _chunkPrefab;
    [SerializeField] float _chunkLength = 10;
    [SerializeField] float[] _lanes = { -3f, 0f, 3f };
    [SerializeField] float[] _horizontalLanes = { -3f, 0f, 3f };

    public Chunk[] ChunkPrefab => _chunkPrefab;
    public float ChunkLength => _chunkLength;
    public float[] Lanes => _lanes;
    public float[] HorizontalLanes => _horizontalLanes;

}