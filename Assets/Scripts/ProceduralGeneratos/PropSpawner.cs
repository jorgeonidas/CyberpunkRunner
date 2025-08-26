using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    [Tooltip("List of possible props (ATM, trash bin, mailbox, etc).")]
    [SerializeField] private List<GameObject> _propPrefabs = new();

    [Tooltip("Optional parent where the spawned prop will be placed.")]
    [SerializeField] private Transform _parent;

    [Tooltip("Generate on Start automatically.")]
    [SerializeField] private bool _spawnOnStart = true;

    private GameObject _currentInstance;

    private void Start()
    {
        if (_spawnOnStart)
            Spawn();
    }

    [ContextMenu("Spawn")]
    public void Spawn()
    {
        if (_propPrefabs == null || _propPrefabs.Count == 0)
        {
            Debug.LogWarning("[PropSpawner] No prefabs assigned.");
            return;
        }

        // Clean previous instance (only one allowed here)
        if (_currentInstance != null)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                DestroyImmediate(_currentInstance);
            else
                Destroy(_currentInstance);
#else
            Destroy(_currentInstance);
#endif
        }

        // Pick random prefab
        int index = Random.Range(0, _propPrefabs.Count);
        GameObject prefab = _propPrefabs[index];

        // Instantiate
        _currentInstance = Instantiate(prefab, transform.position, transform.rotation, _parent);
    }
}
