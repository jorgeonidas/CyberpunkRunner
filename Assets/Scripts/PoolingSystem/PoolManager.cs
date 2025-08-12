using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }
    [Serializable]
    public struct PoolConfig
    {
        public string key;
        public PooledObject objectPrefab;
        public int defaultCapacity;
        public int maxSize;
        public int prewarm;
    }

    [SerializeField] private List<PoolConfig> _configs;
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
        foreach (var configuration in _configs)
        {
            ObjectPool<PooledObject> pool = null;

            pool = new ObjectPool<PooledObject>(
                createFunc: () =>
                {
                    PooledObject poolObject = Instantiate(configuration.objectPrefab);
                    poolObject.Pool ??= pool;     // ?? assing if not assigned
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
                defaultCapacity: Mathf.Max(1, configuration.defaultCapacity),
                maxSize: Mathf.Max(1, configuration.maxSize)
            );

            _pools[configuration.key] = pool;

            // Precalienta si quieres
            for (int i = 0; i < configuration.prewarm; i++)
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
