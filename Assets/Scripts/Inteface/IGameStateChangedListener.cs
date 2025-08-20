using UnityEngine;

public interface IGameStateChangedListener
{
    GameState CurrentGameState { get; set; }
    void OnGameStateChanged(GameState newGameState);
}
