using UnityEngine;

public class PauseMenuPanel : AbstractUIPanel
{
    override public string Id => StringConstants.IngamePanels.Pause;
    GameManager _gameManager;
    private void Start()
    {
        _gameManager = GameManager.Instance;
    }

    public void OnResumeButtonPressed()
    {
        _gameManager.TooglePauseState();
    }
    
    public void OnQuitButtonPressed()
    {
        //gameManager.ToMainMenu();
    }
}
