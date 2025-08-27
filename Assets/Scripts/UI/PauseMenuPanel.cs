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
        SfxManager.Instance.PlayUISfx(SfxIdEnum.UISfxId.back);
    }

    public void OnQuitButtonPressed()
    {
        Hide();
        PopupFactory.ShowConfirmationPopup("Go to main menu", "Are you sure you want to quit to main menu?", "Yes", () =>
        {
            ScenesManager.ToMainMenu();
        }, true, "No", () =>
        {
            Show();
        });
    }
}
