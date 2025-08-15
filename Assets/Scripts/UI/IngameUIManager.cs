using System;
using UnityEngine;

public class IngameUIManager : MonoBehaviour, IUIPanelsOrganizer
{
    [Header("Panels Catalog")]
    [SerializeField] UIPanelCatalog _panelCatalog;
    [Header("Events")]
    [SerializeField] ScoreChangedEvent _scoreChangedEvent;
    [SerializeField] GameStateChangedEvent _gamestateChangedEvent;
    [SerializeField] PowerupActivationEvent _powerupActivationEvent;
    IngameHUD _ingameHud;
    void Awake()
    {
        _panelCatalog.Initialize();
    }
    private void Start()
    {
        if (_panelCatalog.TryGet(StringConstants.IngamePanels.IngameHud, out IUIPanel hudPanel))
        {
            _ingameHud = hudPanel as IngameHUD;
        }
        Hide(StringConstants.IngamePanels.GameOver);
    }

    void OnEnable()
    {
        _scoreChangedEvent.OnEventRaised += OnScoreChanged;
        _gamestateChangedEvent.OnEventRaised += OnGameStateChanged;
        _powerupActivationEvent.OnEventRaised += OnPowerupActivated;
    }

    void OnDisable()
    {
        _scoreChangedEvent.OnEventRaised -= OnScoreChanged;
        _gamestateChangedEvent.OnEventRaised -= OnGameStateChanged;
        _powerupActivationEvent.OnEventRaised -= OnPowerupActivated;    
    }

    private void OnScoreChanged(int score)
    {
        _ingameHud.SetScore(score);
    }

    private void OnPowerupActivated(PowerupBase powerupData)
    {
        _ingameHud.ActivatePowerup(powerupData);
    }

    private void OnGameStateChanged(GameState gameState)
    {
        switch (gameState)
        {
            case GameState.GameOver:
                Show(StringConstants.IngamePanels.GameOver);
                break;
        }
    }

    public void Show(string panelId)
    {
        if (_panelCatalog.TryGet(panelId, out IUIPanel gameOver))
        {
            gameOver.Show();
        }
    }

    public void Hide(string panelId)
    {
        if (_panelCatalog.TryGet(panelId, out IUIPanel gameOver))
        {
            gameOver.Hide();
        }
    }
}
