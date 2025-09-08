using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [SerializeField] private List<PoolObjectConfig> _poolObjectConfigs;
    // [SerializeField] private List<PoolConfig> _configs;
    private readonly Dictionary<string, ObjectPool<PooledObject>> _pools = new();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        SetupPools();
    }

    private void SetupPools()
    {
        foreach (var configuration in _poolObjectConfigs)
        {
            RegisterPool(configuration.Config.key,
                        configuration.Config.objectPrefab,
                        configuration.Config.defaultCapacity,
                        configuration.Config.maxSize);
        }
    }

    public void RegisterPool(string key, PooledObject prefab, int defaultCapacity, int maxSize, Transform parent = null)
    {
        if (_pools.ContainsKey(key))
        {
            return;
        }

        ObjectPool<PooledObject> pool = null;
        pool = new ObjectPool<PooledObject>(
            createFunc: () =>
            {
                var obj = Instantiate(prefab);
                if (obj.Pool == null) obj.Pool = pool;
                obj.SetPoolObjectId(key);
                obj.transform.parent = parent != null ? parent : transform;
                obj.gameObject.SetActive(false);
                return obj;
            },
            actionOnGet: o => { o.gameObject.SetActive(true); o.OnGetFromPool(); },
            actionOnRelease: o => { o.OnReleaseToPool(); o.gameObject.SetActive(false); },
            actionOnDestroy: o => { if (o) Destroy(o.gameObject); },
            collectionCheck: true,
            defaultCapacity: Mathf.Max(1, defaultCapacity),
            maxSize: Mathf.Max(1, maxSize)
        );

        _pools[key] = pool;
    }

    public PooledObject Get(string key)
    {
        var pool = _pools[key];
        return pool.Get();
    }

    public PooledObject Get(string key, Vector3 position, Quaternion rotation)
    {
        var poolObject = Get(key);
        poolObject.transform.position = position;
        poolObject.transform.rotation = rotation;
        return poolObject;
    }
}
