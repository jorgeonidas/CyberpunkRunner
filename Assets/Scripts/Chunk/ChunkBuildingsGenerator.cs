using UnityEngine;

public class ChunkBuildingsGenerator : MonoBehaviour
{
    [SerializeField] Chunk _chunk;
    [SerializeField] BuildingGenerator[] _buildingGeneratos;
    [SerializeField] PropScatterRectangle[] _proprScatters;
    void Start()
    {
        for (int i = 0; i < _proprScatters.Length; i++)
        {
            _proprScatters[i].Generate();
        }
    }

    private void OnEnable()
    {
        _chunk.OnChunkInitialized += GenerateBuildings;
    }

    void OnDisable()
    {
        _chunk.OnChunkInitialized -= GenerateBuildings;
    }

    public void GenerateBuildings()
    {
        for (int i = 0; i < _buildingGeneratos.Length; i++)
        {
            _buildingGeneratos[i].Build();
        }
    }
}
