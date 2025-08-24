using UnityEngine;

public class MainMenuPanel : AbstractUIPanel
{
    public override string Id => "MainMenu";

    public void OnPlayButtonPressed()
    {
        MainMenuManager.Instance.ToGameScene();
    }

    public void OnQuitButtonPressed()
    {
        MainMenuManager.Instance.QuitGame();
    }
}
