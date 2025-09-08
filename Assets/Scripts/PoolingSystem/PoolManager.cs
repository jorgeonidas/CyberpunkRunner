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
            ObjectPool<PooledObject> pool = null;

            pool = new ObjectPool<PooledObject>(
                createFunc: () =>
                {
                    PooledObject poolObject = Instantiate(configuration.Config.objectPrefab);
                    poolObject.Pool ??= pool;     // ?? assing if not assigned
                    poolObject.SetPoolObjectId(configuration.Config.key);
                    poolObject.transform.parent = this.transform;
                    poolObject.gameObject.SetActive(false);
                    return poolObject;
                },
                actionOnGet: (poolObject) =>
                {
                    poolObject.gameObject.SetActive(true);
                    poolObject.OnGetFromPool();
                },
                actionOnRelease: (poolObject) =>
                {
                    poolObject.OnReleaseToPool();
                    poolObject.gameObject.SetActive(false);
                },
                actionOnDestroy: (poolObject) =>
                {
                    if (poolObject)
                    {
                        Destroy(poolObject.gameObject);
                    }
                },
                collectionCheck: true,
                defaultCapacity: Mathf.Max(1, configuration.Config.defaultCapacity),
                maxSize: Mathf.Max(1, configuration.Config.maxSize)
            );

            _pools[configuration.Config.key] = pool;

            // Precalienta si quieres
            for (int i = 0; i < configuration.Config.prewarm; i++)
            {
                var o = pool.Get();
                pool.Release(o);
            }
        }
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
