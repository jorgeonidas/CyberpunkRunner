using UnityEngine;

public class ChunkBuildingsGenerator : MonoBehaviour
{
    [SerializeField] BuildingGenerator[] _buildingGeneratos;
    void Start()
    {
        for (int i = 0; i < _buildingGeneratos.Length; i++)
        {
            _buildingGeneratos[i].Build();
        }

    }
}
