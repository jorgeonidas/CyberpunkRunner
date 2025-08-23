using System;
using UnityEngine;

public class IngameUIManager : MonoBehaviour, IUIPanelsOrganizer
{
    [Header("Panels Catalog")]
    [SerializeField] UIPanelCatalog _panelCatalog;
    [Header("Events")]
    [SerializeField] CoinCollectedEvent _coinCollectedEvent;
    [SerializeField] GameStateChangedEvent _gamestateChangedEvent;
    [SerializeField] PowerupActivationEvent _powerupActivationEvent;
    IngameHUD _ingameHud;
    GameManager _gameManager;

    void Awake()
    {
        _panelCatalog.Initialize();
    }

    private void Start()
    {
        _gameManager = GameManager.Instance;
        if (_panelCatalog.TryGet(StringConstants.IngamePanels.IngameHud, out IUIPanel hudPanel))
        {
            _ingameHud = hudPanel as IngameHUD;
        }
        Hide(StringConstants.IngamePanels.GameOver);
        Hide(StringConstants.IngamePanels.Pause);
    }

    private void Update()
    {
        if (_gameManager == null)
        {
            return;
        }
        if(_gameManager.CurrentGameState != GameState.Playing)
        {
            return;
        }
        _ingameHud.SetTraveledDistance(_gameManager.GetDistanceTravelled());
    }

    void OnEnable()
    {
        _coinCollectedEvent.OnEventRaised += OnScoreChanged;
        _gamestateChangedEvent.OnEventRaised += OnGameStateChanged;
        _powerupActivationEvent.OnEventRaised += OnPowerupActivated;
    }

    void OnDisable()
    {
        _coinCollectedEvent.OnEventRaised -= OnScoreChanged;
        _gamestateChangedEvent.OnEventRaised -= OnGameStateChanged;
        _powerupActivationEvent.OnEventRaised -= OnPowerupActivated; 
    }

    private void OnScoreChanged(int score)
    {
        _ingameHud.SetCoinsPicked(score);
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
            case GameState.Paused:
                Show(StringConstants.IngamePanels.Pause);
                break;
            case GameState.Playing:
                Hide(StringConstants.IngamePanels.Pause);
                break;
        }
    }

    public void Show(string panelId)
    {
        if (_panelCatalog.TryGet(panelId, out IUIPanel panel))
        {
            panel.Show();
        }
    }

    public void Hide(string panelId)
    {
        if (_panelCatalog.TryGet(panelId, out IUIPanel panel))
        {
            panel.Hide();
        }
    }
}
