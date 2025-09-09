using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressablessSetup : MonoBehaviour
{
    public static AddressablessSetup Instance { get; private set; }
    [SerializeField] private AssetLabelReference[] _poolObjectConfigsLabel;
    [SerializeField] private PoolCatalogSO _poolCatalog;
    private readonly List<AsyncOperationHandle<IList<GameObject>>> _groupHandles = new();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public async Task Initialize()
    {
        await Addressables.InitializeAsync().Task;

        var loadedPrefabs = new List<GameObject>();

        foreach (var label in _poolObjectConfigsLabel)
        {
            // Carga todos los GameObjects marcados con ese label (NO instancias de escena)
            var handle = Addressables.LoadAssetsAsync<GameObject>(
                label,
                callback:
                go =>
                {
                    if (go != null)
                    {
                        Debug.Log($"[AddressablesSetup] Loaded prefab: {go.name} with label {label.labelString}");
                        loadedPrefabs.Add(go);
                    }
                },
                releaseDependenciesOnFailure: true
            );

            _groupHandles.Add(handle);
            await handle.Task; // al terminar, los assets están en memoria
        }

        //populate pool catalog
        var poolObjects = new List<PooledObject>();
        foreach (var loadedPrefab in loadedPrefabs)
        {
            var poolObject = loadedPrefab.GetComponent<PooledObject>();
            if (poolObject != null)
            {
                poolObjects.Add(poolObject);
            }
            else
            {
                Debug.LogWarning($"[AddressablesSetup] Loaded prefab {loadedPrefab.name} does not have a PoolConfig component.");
            }
        }

        // Register pool configs
        _poolCatalog.PopulateCatalog(poolObjects);
    }
}
