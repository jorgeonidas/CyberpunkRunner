using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

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

    private readonly Dictionary<string, ObjectPool<GameObject>> _pools = new();
    private readonly List<(GameObject instance, string poolKey)> _activePieces = new();

    private void Awake()
    {
        InitializePoolsFor(baseParts);
        InitializePoolsFor(middleParts);
        InitializePoolsFor(topParts);
    }

    private void InitializePoolsFor(GameObject[] prefabs)
    {
        foreach (var prefab in prefabs)
        {
            if (prefab == null) continue;

            var key = prefab.name;
            if (_pools.ContainsKey(key)) continue;

            _pools.Add(key, new ObjectPool<GameObject>(
                createFunc: () => Instantiate(prefab),
                actionOnGet: (obj) =>
                {
                    obj.SetActive(true);
                    obj.transform.SetParent(transform);
                },
                actionOnRelease: (obj) => obj.SetActive(false),
                actionOnDestroy: (obj) => Destroy(obj),
                collectionCheck: false,
                defaultCapacity: 10,
                maxSize: 50));
        }
    }

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
        if (pieceArray == null || pieceArray.Length == 0)
        {
            return 0;
        }

        GameObject prefab = pieceArray[Random.Range(0, pieceArray.Length)];
        if (prefab == null)
        {
            return 0;
        }

        string key = prefab.name;
        if (!_pools.ContainsKey(key))
        {
            Debug.LogError($"Pool for '{key}' not found. Make sure it is in the prefab arrays.");
            return 0;
        }

        GameObject clone = _pools[key].Get();
        _activePieces.Add((clone, key));
        clone.transform.position = this.transform.position + new Vector3(0, inputHeight, 0);
        clone.transform.rotation = transform.rotation;
        
        float heightOffset = 0;
        if (clone.TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
        {
            Mesh cloneMesh = meshFilter.mesh;
            Bounds bounds = cloneMesh.bounds;
            heightOffset = bounds.size.y;
        }
        
        SetMaterialRecursively(clone, _pickedMaterial);
        
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
        if (_pools.Count == 0 && Application.isEditor && !Application.isPlaying)
        {
            while (transform.childCount > 0)
            {
                DestroyImmediate(transform.GetChild(0).gameObject);
            }
            return;
        }

        foreach (var piece in _activePieces)
        {
            if (_pools.TryGetValue(piece.poolKey, out var pool))
            {
                pool.Release(piece.instance);
            }
            else
            {
                Debug.LogWarning($"Pool with key '{piece.poolKey}' not found. The object will be destroyed.");
                Destroy(piece.instance);
            }
        }
        _activePieces.Clear();
    }
}
