using DG.Tweening;
using TMPro;
using UnityEngine;

public class IngameHUD : AbstractUIPanel
{
    [SerializeField] TachometerUI _tachometerUI;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] TextMeshProUGUI _distanceTraveledText;
    [SerializeField] PowerupUIManager _powerupUIManager;
    public override string Id => StringConstants.IngamePanels.IngameHud;
    //private Tweener _distanceTween;
    public void SetCoinsPicked(int score)
    {
        _scoreText.text = $"Coins: {score}";
    }

    public void SetTraveledDistance(int distance)
    {
         _distanceTraveledText.SetText("{0}m", distance);
    }

    public void ActivatePowerup(PowerupBase powerup)
    {
        _powerupUIManager.ActivatePowerup(powerup);
    }
}
