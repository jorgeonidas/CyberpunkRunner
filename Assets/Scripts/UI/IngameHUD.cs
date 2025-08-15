using TMPro;
using UnityEngine;

public class IngameHUD : AbstractUIPanel
{
    [SerializeField] TachometerUI _tachometerUI;
    [SerializeField] TextMeshProUGUI _scoreText;
    [SerializeField] PowerupUIManager _powerupUIManager;


    public override string Id => StringConstants.IngamePanels.IngameHud;

    public void SetScore(int score)
    {
        _scoreText.text = $"Score: {score}";
    }

    public void ActivatePowerup(PowerupBase powerup)
    {
        _powerupUIManager.ActivatePowerup(powerup);
    }
}
