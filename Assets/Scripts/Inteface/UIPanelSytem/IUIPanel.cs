using UnityEngine;

public interface IUIPanel
{
    string Id { get; }           // "Pause", "Hud", "GameOver", etc.
    void Show();
    void Hide();
}
