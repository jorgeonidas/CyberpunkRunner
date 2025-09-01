using UnityEngine;

public class ChunkBuildingsGenerator : MonoBehaviour
{
    [SerializeField] BuildingGenerator[] _buildingGeneratos;
    [SerializeField] PropScatterRectangle[] _proprScatters;
    void Start()
    {
        GenerateBuildings();
        for (int i = 0; i < _proprScatters.Length; i++)
        {
            _proprScatters[i].Generate();
        }
    }

    private void OnEnable()
    {
        GenerateBuildings();
    }

    private void GenerateBuildings()
    {
        for (int i = 0; i < _buildingGeneratos.Length; i++)
        {
            _buildingGeneratos[i].Build();
        }
    }
}
