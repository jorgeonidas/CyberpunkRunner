using UnityEngine;

public class ChunkBuildingsGenerator : MonoBehaviour
{
    [SerializeField] BuildingGenerator[] _buildingGeneratos;
    [SerializeField] PropScatterRectangle[] _proprScatters;
    void Start()
    {
        for (int i = 0; i < _buildingGeneratos.Length; i++)
        {
            _buildingGeneratos[i].Build();
        }
        for (int i = 0; i < _proprScatters.Length; i++)
        {
            _proprScatters[i].Generate();
        }
    }
}
