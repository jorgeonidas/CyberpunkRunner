using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

[CreateAssetMenu(fileName = "PoolCatalogSO", menuName = "Sci-FiRunner/PoolCatalogSO")]
public class PoolCatalogSO : ScriptableObject
{
    [SerializeField] private int defaultCapacity;
    [SerializeField] private int maxSize;
    [SerializeField] private List<PoolConfig> poolConfigs = new List<PoolConfig>();
    
    public int DefaultCapacity => defaultCapacity;
    public int MaxSize => maxSize;
    public List<PoolConfig> PoolConfigs => poolConfigs;

    public void PopulateCatalog(List<PooledObject> poolObjectsPrefabs)
    {   
        poolConfigs.Clear();
        foreach (var poolObject in poolObjectsPrefabs)
        {
            var config = new PoolConfig()
            {
                key = poolObject.gameObject.name,
                objectPrefab = poolObject,
                defaultCapacity = defaultCapacity,
                maxSize = maxSize
            };
            poolConfigs.Add(config);
        }
    }
}
