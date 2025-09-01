using System;
using UnityEngine;

public class MainMenuUIManager : UIPanelsOrganizer
{
    public static MainMenuUIManager Instance { get; private set; }
    protected override void Awake()
    {
        base.Awake();
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
    }
    private void Start()
    {
        ShowMainMenu();
    }

    public void ShowMainMenu()
    {
        ShowPanelById(StringConstants.MainMenuPanels.MainMenu);
    }

    public void ShowSettingsMenu()
    {
        Show(StringConstants.MainMenuPanels.SettingsMenu);
    }

    public void ShowShopPanel()
    {
        ShowPanelById(StringConstants.MainMenuPanels.ShopMenu);
    }

    public void ShowPanelById(String panelId)
    {
        _panelCatalog.HideAllPanels();
        Show(panelId);
    }
}
