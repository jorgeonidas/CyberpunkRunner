using System;
using UnityEngine;

[Serializable]
public struct PoolConfig
{
    public string key;
    public PooledObject objectPrefab;
    public int defaultCapacity;
    public int maxSize;
}
[CreateAssetMenu(fileName = "PoolObjectConfig", menuName = "Sci-FiRunner/PoolObjectConfig")]
public class PoolObjectConfig : ScriptableObject
{
    [SerializeField] private PoolConfig _poolObjectConfig;
    public PoolConfig Config => _poolObjectConfig;
}
