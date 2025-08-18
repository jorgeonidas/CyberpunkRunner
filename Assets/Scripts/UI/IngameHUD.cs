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
    float _distanceTraveled = 0;
    //private Tweener _distanceTween;      // para evitar tweens acumulados
    public void SetScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void SetTraveledDistance(float distance)
    {
        // _distanceTween?.Kill();
        // _distanceTween = DOVirtual.Float(
        //     _distanceTraveled, distance, 0.2f,
        //     x =>
        //     {
        //         _distanceTraveled = x;
        //         _distanceTraveledText.SetText("{0}m", Mathf.RoundToInt(x));
        //     }
        // )
        // .SetEase(Ease.OutQuad)
        // .SetLink(gameObject);
         _distanceTraveledText.SetText("{0}m", Mathf.RoundToInt(distance));
    }

    public void ActivatePowerup(PowerupBase powerup)
    {
        _powerupUIManager.ActivatePowerup(powerup);
    }
}
