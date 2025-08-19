using UnityEngine;

[CreateAssetMenu(fileName = "CoinCollectedEvent", menuName = "Game Events/ScoreChangedEvent")]
public class CoinCollectedEvent : EventChannel<int> { }