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
        foreach (var cfg in _configs)
        {
            ObjectPool<PooledObject> pool = null;

            pool = new ObjectPool<PooledObject>(
                createFunc: () =>
                {
                    var obj = Instantiate(cfg.objectPrefab);
                    // Poner inactivo al crear evita parpadeos antes de "Get".
                    obj.gameObject.SetActive(false);
                    return obj;
                },
                actionOnGet: (obj) =>
                {
                    obj.Pool ??= pool;     // asigna referencia al pool
                    obj.gameObject.SetActive(true);
                    obj.OnGetFromPool();
                },
                actionOnRelease: (obj) =>
                {
                    obj.OnReleaseToPool();
                    obj.gameObject.SetActive(false);
                },
                actionOnDestroy: (obj) =>
                {
                    if (obj) Destroy(obj.gameObject);
                },
                collectionCheck: true,
                defaultCapacity: Mathf.Max(1, cfg.defaultCapacity),
                maxSize: Mathf.Max(1, cfg.maxSize)
            );

            _pools[cfg.key] = pool;

            // Precalienta si quieres
            for (int i = 0; i < cfg.prewarm; i++)
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
