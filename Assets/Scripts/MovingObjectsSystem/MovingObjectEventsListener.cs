using System;
using UnityEngine;

public class MovingObjectEventsListener : MonoBehaviour
{
    [Header("Events to liste")]
    [SerializeField] OnPositionEvent _placeCoinEvent;
    [SerializeField] MovingObjectsSpawner _movingObjectsSpawner;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        _placeCoinEvent.OnEventRaised += PlaceCoin;
    }

    void OnDisable()
    {

        _placeCoinEvent.OnEventRaised -= PlaceCoin;
    }

    private void PlaceCoin(Vector3 position)
    {
        _movingObjectsSpawner.SpawnCoin(position);
    }
}
