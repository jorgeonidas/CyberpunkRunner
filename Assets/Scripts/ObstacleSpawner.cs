using System.Collections;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] _obstaclePrefabs;
    [SerializeField] Transform _obstacleParent;
    [SerializeField] float _timeBetweenSpawnObstacles = 0.5f;
    [SerializeField] float _spawnWitdth = 4f;
    private void Start()
    {
        StartCoroutine(SpawnObstaclesRoutine());
    }

    IEnumerator SpawnObstaclesRoutine()
    {
        while (true)
        {
            Vector3 spawnPosition = new Vector3(
                                    transform.position.x + Random.Range(-_spawnWitdth, _spawnWitdth),
                                    transform.position.y, transform.position.z);
            yield return new WaitForSeconds(_timeBetweenSpawnObstacles);
            Instantiate(_obstaclePrefabs[Random.Range(0, _obstaclePrefabs.Length)], spawnPosition, Random.rotation, _obstacleParent);
        }
    }
}
