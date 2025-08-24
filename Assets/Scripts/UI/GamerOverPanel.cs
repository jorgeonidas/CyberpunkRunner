using TMPro;
using UnityEngine;

public class GamerOverPanel : AbstractUIPanel
{
    [SerializeField] TextMeshProUGUI _traveledDistanceText;
    [SerializeField] GameObject _newRecordGameobject;
    [SerializeField] TextMeshProUGUI _earnedCoinsText;
    public override string Id => StringConstants.IngamePanels.GameOver;
    GameManager _gameManager => GameManager.Instance;
    
    private void Awake()
    {

    }

    public override void Show()
    {
        int lastRecordDistance = PlayerDataManager.GetRecordDistance();
        int traveledDistance = _gameManager.GetDistanceTravelled();
        bool isNewRecord = traveledDistance > lastRecordDistance;
        _newRecordGameobject.SetActive(isNewRecord && lastRecordDistance > 0);
        if (isNewRecord)
        {
            PlayerDataManager.SetRecordDistance(traveledDistance);
            PlayerDataManager.SaveData();
        }
        _traveledDistanceText.text = $"DISTANCE: {traveledDistance}m";
        _earnedCoinsText.text = $"COINS: {_gameManager.CoinsCollected}";
        base.Show();
    }

    public void Retry()
    {
        _gameManager.SaveCollectedCoins();
        ScenesManager.ToGameScene();
    }

    public void GoToMainMenu()
    {
        Hide();
        PopupFactory.ShowConfirmationPopup("Go to main menu", "Are you sure you want to quit to main menu?", "Yes", () =>
        {
            _gameManager.SaveCollectedCoins();
            ScenesManager.ToMainMenu();
        }, true, "No", () =>
        {
            Show();
        });
    }
}
