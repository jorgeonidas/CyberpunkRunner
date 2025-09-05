using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RocketLauncher : MonoBehaviour
{

    // Usar variables locales en vez de campos para evitar problemas con corrutinas simultáneas
    float _zOffset = 5f;
    // Estado para lanzamiento por Update
    private List<Transform> _pendingTargets = null;
    private float _pendingTimeBetweenRockets = 0f;
    private string _pendingRocketId = null;
    private int _pendingRocketIndex = 0;
    private float _pendingNextLaunchTime = 0f;

    public void LaunchRockets(string rocketId, float timeBetweenRockets, int numberOfRockets)
    {
        var obstacles = FindObjectsByType<DestructibleObstacle>(FindObjectsSortMode.None);
        float currentZ = transform.position.z + _zOffset;
        var filteredObstacles = obstacles.Where(x => !x.IsDestroyed && x.transform.position.z > currentZ).ToList();
        var selectedObstacles = filteredObstacles.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).Take(numberOfRockets).ToList();
        var targets = selectedObstacles.Select(x => x.transform).ToList();

        // Evitar duplicados en _pendingTargets
        if (_pendingTargets != null && _pendingTargets.Count > 0)
        {
            targets = targets.Where(t => !_pendingTargets.Contains(t)).ToList();
        }

        if (targets.Count == 0)
        {
            Debug.LogWarning("RocketLauncher: No new obstacles to launch rockets.");
            return;
        }
        _pendingTargets = targets;
        _pendingTimeBetweenRockets = timeBetweenRockets;
        _pendingRocketId = rocketId;
        _pendingRocketIndex = 0;
        _pendingNextLaunchTime = Time.time;
    }

    // Elimina la corrutina, ahora el lanzamiento es por Update
    private void Update()
    {
        if (_pendingTargets != null && _pendingRocketIndex < _pendingTargets.Count)
        {
            if (Time.time >= _pendingNextLaunchTime)
            {
                var target = _pendingTargets[_pendingRocketIndex];
                LaunchRocket(target, _pendingRocketId);
                _pendingRocketIndex++;
                _pendingNextLaunchTime = Time.time + _pendingTimeBetweenRockets;
                if (_pendingRocketIndex >= _pendingTargets.Count)
                {
                    // Lanzamiento terminado
                    _pendingTargets = null;
                    _pendingRocketId = null;
                }
            }
        }
    }

    private void LaunchRocket(Transform target, string rocketId)
    {
        // Instantiate and launch the rocket towards the target position
        var poolObject = PoolManager.Instance.Get(rocketId);
        if (poolObject == null)
        {
            Debug.LogWarning($"RocketLauncher: No rockets available in the pool for '{rocketId}'");
            return;
        }
        Rocket rocketInstance = poolObject as Rocket;
        if (rocketInstance == null)
        {
            Debug.LogError($"RocketLauncher: The object obtained from the pool is not of type Rocket (id: {rocketId})");
            return;
        }
        rocketInstance.transform.position = transform.position;
        rocketInstance.Launch(target);
    }
}
