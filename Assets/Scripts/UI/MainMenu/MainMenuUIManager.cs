using System;
using UnityEngine;

public class MainMenuUIManager : UIPanelsOrganizer
{
    private void Start()
    {
        Show(StringConstants.MainMenuPanels.MainMenu);
        Hide(StringConstants.MainMenuPanels.SettingsMenu);
    }

    public void ShowSettingsMenu()
    {
        Show(StringConstants.MainMenuPanels.SettingsMenu);
    }
}
