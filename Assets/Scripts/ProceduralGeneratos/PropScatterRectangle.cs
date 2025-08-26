using System.Collections.Generic;
using UnityEngine;


public class PropScatterRectangle : MonoBehaviour
{
    [Header("Prefabs")]
    [Tooltip("Collection of props to scatter. A random one will be chosen for each instance.")]
    [SerializeField] private List<GameObject> _propPrefabs = new();

    [Header("Area (local space)")]
    [Tooltip("Rectangle size in meters (X = width, Y = length). Centered on this transform.")]
    [SerializeField] private Vector2 _size = new Vector2(12f, 3f);

    [Header("Distribution")]
    [Tooltip("Desired instances per square meter. Final count = area(m^2) * density.")]
    [Min(0f)]
    [SerializeField] private float _densityPerSquareMeter = 0.2f;

    [Tooltip("Minimum separation between spawned instances (XZ plane).")]
    [Min(0f)]
    [SerializeField] private float _minSeparation = 0.5f;

    [Tooltip("Max attempts factor to find non-overlapping positions (safety for rejection sampling).")]
    [Min(1)]
    [SerializeField] private int _attemptsMultiplier = 15;

    [Header("Randomization")]
    [SerializeField] private int _seed = 12345;
    [SerializeField] private bool _autoRandomSeed = false;
    [Space]
    [Tooltip("Random Y rotation range for variation.")]
    [SerializeField] private Vector2 _yRotationRange = new Vector2(0f, 360f);
    [Tooltip("Uniform random scale range applied to each instance.")]
    [SerializeField] private Vector2 _uniformScaleRange = new Vector2(1f, 1f);

    [Header("Grounding (optional)")]
    [SerializeField] private bool _alignToGround = true;
    [SerializeField] private float _raycastHeight = 3f;
    [SerializeField] private LayerMask _groundMask = ~0;

    [Header("Gizmos")]
    [SerializeField] private bool _showPreviewPoints = true;
    [SerializeField] private Color _areaColor = new Color(0f, 0.7f, 1f, 0.15f);
    [SerializeField] private Color _wireColor = new Color(0f, 0.7f, 1f, 0.9f);
    [SerializeField] private Color _previewPointColor = new Color(0.9f, 0.9f, 0.2f, 0.9f);

    private const string ContainerName = "_ScatterRuntime";
    private Transform _container;

    // ---------- Public API ----------
    [ContextMenu("Generate")]
    public void Generate()
    {
        if (_propPrefabs == null || _propPrefabs.Count == 0)
        {
            Debug.LogWarning("[PropScatterRectangle] No prefabs assigned.");
            return;
        }

        if (_autoRandomSeed)
        {
            _seed = GenerateSeed();
        }


        EnsureContainer();
        ClearContainerChildren();

        var prng = new System.Random(_seed);

        int targetCount = ComputeTargetCount();
        if (targetCount <= 0) return;

        var accepted = new List<Vector3>(targetCount);
        int attemptsLeft = targetCount * Mathf.Max(1, _attemptsMultiplier);

        while (accepted.Count < targetCount && attemptsLeft-- > 0)
        {
            var local = RandomPointInRect(prng);
            if (IsFarEnough(local, accepted, _minSeparation))
            {
                accepted.Add(local);
            }
        }

        // Instantiate
        foreach (var local in accepted)
        {
            var prefab = _propPrefabs[prng.Next(_propPrefabs.Count)];
            if (prefab == null) continue;

            // Random rotation & scale
            float yRot = Mathf.Lerp(_yRotationRange.x, _yRotationRange.y, (float)prng.NextDouble());
            float scale = Mathf.Lerp(_uniformScaleRange.x, _uniformScaleRange.y, (float)prng.NextDouble());

            // World position from local (XZ)
            Vector3 worldPos = transform.TransformPoint(local);

            Quaternion rot = Quaternion.Euler(0f, yRot, 0f);
            if (_alignToGround)
            {
                if (TryProjectToGround(worldPos, out var groundedPos, out var groundRot))
                {
                    worldPos = groundedPos;
                    rot = Quaternion.Euler(0f, yRot, 0f) * groundRot;
                }
            }

            var go = (Application.isPlaying)
                ? Instantiate(prefab, worldPos, rot, _container)
                : (GameObject)UnityEditor.PrefabUtility.InstantiatePrefab(prefab, _container);

            go.transform.position = worldPos;
            go.transform.rotation = rot;
            go.transform.localScale *= scale;
        }
    }

    [ContextMenu("Clear Generated")]
    public void ClearGenerated()
    {
        EnsureContainer();
        ClearContainerChildren();
    }

    // ---------- Internals ----------
    private int ComputeTargetCount()
    {
        float area = Mathf.Max(0f, _size.x) * Mathf.Max(0f, _size.y);
        return Mathf.RoundToInt(area * Mathf.Max(0f, _densityPerSquareMeter));
    }

    private Vector3 RandomPointInRect(System.Random prng)
    {
        float halfX = _size.x * 0.5f;
        float halfZ = _size.y * 0.5f;
        float x = Mathf.Lerp(-halfX, halfX, (float)prng.NextDouble());
        float z = Mathf.Lerp(-halfZ, halfZ, (float)prng.NextDouble());
        return new Vector3(x, 0f, z);
    }

    private static bool IsFarEnough(Vector3 candidateLocal, List<Vector3> accepted, float minSep)
    {
        if (minSep <= 0f || accepted.Count == 0) return true;
        float minSqr = minSep * minSep;

        for (int i = 0; i < accepted.Count; i++)
        {
            // XZ plane distance
            Vector3 a = accepted[i];
            float dx = a.x - candidateLocal.x;
            float dz = a.z - candidateLocal.z;
            if ((dx * dx + dz * dz) < minSqr) return false;
        }
        return true;
    }

    private bool TryProjectToGround(Vector3 startWorld, out Vector3 hitPoint, out Quaternion alignRotation)
    {
        Vector3 origin = startWorld + Vector3.up * _raycastHeight;
        Vector3 dir = Vector3.down;
        float dist = _raycastHeight * 2f;

        if (Physics.Raycast(origin, dir, out var hit, dist, _groundMask, QueryTriggerInteraction.Ignore))
        {
            hitPoint = hit.point;
            alignRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
            return true;
        }

        hitPoint = startWorld;
        alignRotation = Quaternion.identity;
        return false;
    }

    private void EnsureContainer()
    {
        if (_container != null) return;

        var t = transform.Find(ContainerName);
        if (t == null)
        {
            var go = GameObject.Find(ContainerName);
            if (go != null && go.transform.parent == transform)
            {
                _container = go.transform;
            }
            else
            {
                var containerGO = new GameObject(ContainerName);
                _container = containerGO.transform;
                _container.SetParent(transform, false);
            }
        }
        else
        {
            _container = t;
        }
    }

    private void ClearContainerChildren()
    {
        if (_container == null) return;

#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
            // Safe destroy in edit mode
            var toDestroy = new List<GameObject>();
            for (int i = _container.childCount - 1; i >= 0; i--)
                toDestroy.Add(_container.GetChild(i).gameObject);

            foreach (var go in toDestroy)
                UnityEditor.Undo.DestroyObjectImmediate(go);

            return;
        }
#endif
        for (int i = _container.childCount - 1; i >= 0; i--)
            Destroy(_container.GetChild(i).gameObject);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        _size.x = Mathf.Max(0f, _size.x);
        _size.y = Mathf.Max(0f, _size.y);
        _uniformScaleRange.x = Mathf.Max(0.01f, _uniformScaleRange.x);
        _uniformScaleRange.y = Mathf.Max(_uniformScaleRange.x, _uniformScaleRange.y);
        _yRotationRange.y = Mathf.Max(_yRotationRange.x, _yRotationRange.y);
    }
#endif

    private void OnDrawGizmosSelected()
    {
        // Draw rectangle area (local)
        Gizmos.matrix = transform.localToWorldMatrix;

        // Filled
        Gizmos.color = _areaColor;
        Gizmos.DrawCube(Vector3.zero, new Vector3(_size.x, 0.01f, _size.y));

        // Wire
        Gizmos.color = _wireColor;
        Gizmos.DrawWireCube(Vector3.zero, new Vector3(_size.x, 0.01f, _size.y));

        // Optional preview points
        if (_showPreviewPoints)
        {
            var prng = new System.Random(_seed);
            int previewCount = Mathf.Min(ComputeTargetCount(), 500); // guard to avoid too many handles
            Gizmos.color = _previewPointColor;

            var accepted = new List<Vector3>(previewCount);
            int attemptsLeft = previewCount * Mathf.Max(1, _attemptsMultiplier);
            while (accepted.Count < previewCount && attemptsLeft-- > 0)
            {
                var p = RandomPointInRect(prng);
                if (IsFarEnough(p, accepted, _minSeparation))
                    accepted.Add(p);
            }

            float r = Mathf.Max(0.03f, _minSeparation * 0.15f);
            foreach (var p in accepted)
                Gizmos.DrawSphere(p, r);
        }

        Gizmos.matrix = Matrix4x4.identity;
    }

    private static int GenerateSeed()
    {
        // Mix 3 different sources of randomness
        unchecked
        {
            return System.Environment.TickCount
                 ^ System.DateTime.Now.Ticks.GetHashCode()
                 ^ UnityEngine.Random.Range(int.MinValue, int.MaxValue);
        }
    }
}
