using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//credits to: https://github.com/mirrorfishmedia/ImaginaryCities
public class BuildingGenerator : MonoBehaviour
{
    public int minPieces = 5;
    public int maxPieces = 20;
    public Material[] materialVariants;
    public GameObject[] baseParts;
    public GameObject[] middleParts;
    public GameObject[] topParts;
    private Material _pickedMaterial;

    [ContextMenu("Generate")]
    public void Build()
    {
        ClearBuilding();

        int targetPieces = Random.Range(minPieces, maxPieces);
        _pickedMaterial = materialVariants[Random.Range(0, materialVariants.Length)];
        float heightOffset = 0;
        heightOffset += SpawnPieceLayer(baseParts, heightOffset);

        for (int i = 2; i < targetPieces; i++)
        {
            heightOffset += SpawnPieceLayer(middleParts, heightOffset);
        }

        SpawnPieceLayer(topParts, heightOffset);
    }

    [ContextMenu("Clear Generated")]
    public void ClearGenerate()
    {
        ClearBuilding();
    }

    float SpawnPieceLayer(GameObject[] pieceArray, float inputHeight)
    {
        Transform randomTransform = pieceArray[Random.Range(0, pieceArray.Length)].transform;
        GameObject clone = Instantiate(randomTransform.gameObject, this.transform.position
            + new Vector3(0, inputHeight, 0), transform.rotation) as GameObject;
        //the roof will have a special case where is an empty object
        float heightOffset = 0;
        if (clone.TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
        {
            Mesh cloneMesh = meshFilter.mesh;
            Bounds bounds = cloneMesh.bounds;
            heightOffset = bounds.size.y;
        }
        SetMaterialRecursively(clone, _pickedMaterial);
        clone.transform.SetParent(this.transform);
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
        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            //when is called from context menu
            if (Application.isEditor && !Application.isPlaying)
            {
                DestroyImmediate(child.gameObject);
            }
            else
            {
                Destroy(child.gameObject);
            }
        }
    }
}
