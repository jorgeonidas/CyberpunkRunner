using TMPro;
using UnityEngine;

public class MainMenuPanel : AbstractUIPanel
{
    [Header("High Score Display")]
    [SerializeField] GameObject _highScoreContainer;
    [SerializeField] TextMeshProUGUI _highScoreText;
    public override string Id => StringConstants.MainMenuPanels.MainMenu;

    private void Start()
    {
        TryDisplayHiScore();
    }

    private void TryDisplayHiScore()
    {
        int highScore = PlayerDataManager.GetRecordDistance();
        _highScoreContainer.SetActive(highScore > 0);
        _highScoreText.text = $"High Score:\n{highScore}m";
    }

    public void OnPlayButtonPressed()
    {
        MainMenuManager.Instance.ToGameScene();
    }

    public void OnQuitButtonPressed()
    {
        MainMenuManager.Instance.QuitGame();
    }
}
