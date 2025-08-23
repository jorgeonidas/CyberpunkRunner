using TMPro;
using UnityEngine;

public class GamerOverPanel : AbstractUIPanel
{
    [SerializeField] TextMeshProUGUI _traveledDistanceText;
    [SerializeField] GameObject _newRecordGameobject;
    [SerializeField] TextMeshProUGUI _earnedCoinsText;
    public override string Id => StringConstants.IngamePanels.GameOver;
    SceneReload _sceneReload;
    GameManager _gameManager => GameManager.Instance;
    private void Awake()
    {
        _sceneReload = GetComponent<SceneReload>();
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
        _sceneReload.ReloadCurrentScene();//TODO MAKE A SCENE MANAGER
        _gameManager.SaveCollectedCoins();
    }

    public void GoToMainMenu()
    {
        // _sceneReload.LoadScene(StringConstants.Scenes.MainMenu);//
        _gameManager.SaveCollectedCoins();
    }
}
