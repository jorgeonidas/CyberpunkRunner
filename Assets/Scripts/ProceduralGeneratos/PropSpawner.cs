using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    [Tooltip("Lista de posibles props (cajero, papelera, buzón, etc).")]
    [SerializeField] private List<GameObject> _propPrefabs = new();

    [Tooltip("Padre opcional donde se colocará el prop generado.")]
    [SerializeField] private Transform _parent;

    [Tooltip("Generate on Start automatically.")]
    [SerializeField] private bool _spawnOnStart = true;

    // Lista para almacenar los props preinstanciados
    private readonly List<GameObject> _propInstances = new();
    private GameObject _currentInstance;

    private void Start()
    {
        PrewarmProps();

        if (_spawnOnStart)
        {
            Spawn();
        }
    }

    /// <summary>
    /// Instancia todos los prefabs de props, los desactiva y los guarda para su uso posterior.
    /// </summary>
    private void PrewarmProps()
    {
        // Primero, limpiamos cualquier instancia existente para evitar duplicados.
        foreach (var instance in _propInstances)
        {
            if (instance != null)
            {
#if UNITY_EDITOR
                if (!Application.isPlaying)
                    DestroyImmediate(instance);
                else
                    Destroy(instance);
#else
                Destroy(instance);
#endif
            }
        }
        _propInstances.Clear();

        if (_propPrefabs == null) return;

        // Instanciamos cada prefab y lo añadimos a nuestra lista de instancias.
        foreach (var prefab in _propPrefabs)
        {
            if (prefab == null)
            {
                Debug.LogWarning("[PropSpawner] Un prefab en la lista es nulo y será omitido.", this);
                continue;
            }

            GameObject instance = Instantiate(prefab, transform.position, transform.rotation, _parent);
            instance.SetActive(false);
            _propInstances.Add(instance);
        }
    }

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        // En modo editor, si llamamos a Spawn desde el menú contextual, puede que necesitemos preinstanciar primero.
#if UNITY_EDITOR
        if (!Application.isPlaying && (_propInstances.Count != _propPrefabs.Count))
        {
            PrewarmProps();
        }
#endif

        if (_propInstances.Count == 0)
        {
            if (_propPrefabs.Count > 0)
                Debug.LogWarning("[PropSpawner] Los props aún no han sido preinstanciados. Llama a PrewarmProps() o espera a Start().", this);
            else
                Debug.LogWarning("[PropSpawner] No hay prefabs asignados.", this);
            return;
        }

        // Ocultamos la instancia anterior.
        if (_currentInstance != null)
        {
            _currentInstance.SetActive(false);
        }

        // Elegimos una instancia aleatoria de nuestra lista preinstanciada y la mostramos.
        int index = Random.Range(0, _propInstances.Count);
        _currentInstance = _propInstances[index];

        if (_currentInstance != null)
        {
            _currentInstance.SetActive(true);
        }
    }
}
