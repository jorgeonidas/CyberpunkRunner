using System.Collections.Generic;
using UnityEngine;

//credits to: https://github.com/mirrorfishmedia/ImaginaryCities
public class BuildingGenerator : MonoBehaviour
{
    public int minPieces = 5;
    public int maxPieces = 20;
    public Material[] materialVariants;
    public string[] baseKeys;
    public string[] middleKeys;
    public string[] topKeys;
    private Material _pickedMaterial;

    private readonly List<(PooledObject instance, string poolKey)> _activePieces = new();


    [ContextMenu("Generate")]
    public void Build()
    {
        ClearBuilding();

        int targetPieces = Random.Range(minPieces, maxPieces);
        _pickedMaterial = materialVariants[Random.Range(0, materialVariants.Length)];
        float heightOffset = 0;

        // Base
        heightOffset += SpawnPieceLayer(baseKeys, heightOffset);

        // Middles
        for (int i = 2; i < targetPieces; i++)
        {
            heightOffset += SpawnPieceLayer(middleKeys, heightOffset);
        }

        // Top
        SpawnPieceLayer(topKeys, heightOffset);
    }

    [ContextMenu("Clear Generated")]
    public void ClearGenerate()
    {
        ClearBuilding();
    }


    float SpawnPieceLayer(string[] keysArray, float inputHeight)
    {
        if (keysArray == null || keysArray.Length == 0)
        {
            return 0;
        }

        int idx = Random.Range(0, keysArray.Length);
        string key = keysArray[idx];
        var pooledObj = PoolManager.Instance.Get(key, this.transform.position + new Vector3(0, inputHeight, 0), transform.rotation);
        pooledObj.transform.SetParent(this.transform); // Asignar como hijo para que siga el movimiento
        _activePieces.Add((pooledObj, key));

        float heightOffset = 0;
        if (pooledObj.TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
        {
            Mesh cloneMesh = meshFilter.mesh;
            Bounds bounds = cloneMesh.bounds;
            heightOffset = bounds.size.y;
        }

        SetMaterialRecursively(pooledObj.gameObject, _pickedMaterial);

        return heightOffset;
    }

    void SetMaterialRecursively(GameObject obj, Material material)
    {
        if (obj.TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer))
        {
            meshRenderer.material = material;
        }
        foreach (Transform child in obj.transform)
        {
            SetMaterialRecursively(child.gameObject, material);
        }
    }


    private void ClearBuilding()
    {
        foreach (var piece in _activePieces)
        {
            // Devuelve al pool central
            var pooledObj = piece.instance.GetComponent<PooledObject>();
            if (pooledObj != null)
            {
                pooledObj.Release();
            }
            else
            {
                Destroy(piece.instance);
            }
        }
        _activePieces.Clear();
    }
}
