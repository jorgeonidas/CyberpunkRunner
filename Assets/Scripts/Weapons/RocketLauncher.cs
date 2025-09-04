using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class RocketLauncher : MonoBehaviour
{

    float _timeBetweenRockets = 0;
    List<Transform> _targets = new List<Transform>();
    float _zOffset = 3f;
    public void LaunchRockets( string rocketId, float timeBetweenRockets, int numberOfRockets)
    {
        // _rocketToLaunch = rocketPrefab;
        _timeBetweenRockets = timeBetweenRockets;

        // Buscar obstáculos destruibles activos en la escena
        var obstacles = FindObjectsByType<DestructibleObstacle>(FindObjectsSortMode.None);
        // Filtrar solo los que estén adelante y cerca
        float currentZ = transform.position.z + _zOffset;
        var filteredObstacles = obstacles.Where(x => x.transform.position.z > currentZ).ToList();
        // Seleccionar aleatoriamente la cantidad igual a numberOfRockets
        var selectedObstacles = filteredObstacles.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).Take(numberOfRockets).ToList();
        _targets = selectedObstacles.Select(x => x.transform).ToList();
        StartCoroutine(LaunchRocketsCoroutine(rocketId));
    }

    private IEnumerator LaunchRocketsCoroutine(string rocketId)
    {
        foreach (var target in _targets)
        {
            LaunchRocket(target, rocketId);
            yield return new WaitForSeconds(_timeBetweenRockets);
        }
    }

    private void LaunchRocket(Transform target, string rocketId)
    {
        // Instantiate and launch the rocket towards the target position
        Rocket rocketInstance = PoolManager.Instance.Get(rocketId) as Rocket;
        rocketInstance.transform.position = transform.position;
        rocketInstance.Launch(target);
    }
}
