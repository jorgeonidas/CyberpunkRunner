using TMPro;
using UnityEngine;

public class CurrencyDisplayer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currencyText;

    private void Start()
    {
        UpdateCurrencyDisplay();
    }

    private void OnEnable()
    {
        PlayerDataManager.OnCurrencyChanged += UpdateCurrencyDisplay;
    }

    private void OnDisable()
    {
        PlayerDataManager.OnCurrencyChanged -= UpdateCurrencyDisplay;
    }

    private void UpdateCurrencyDisplay()
    {
        int currency = PlayerDataManager.GetCoins();
        _currencyText.text = currency.ToString();
    }
}
