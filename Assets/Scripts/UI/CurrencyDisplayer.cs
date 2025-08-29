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
        UserDataServiceSO.Instance.OnCurrencyChanged += UpdateCurrencyDisplay;
    }

    private void OnDisable()
    {
        UserDataServiceSO.Instance.OnCurrencyChanged -= UpdateCurrencyDisplay;
    }

    private void UpdateCurrencyDisplay()
    {
        int currency = UserDataServiceSO.Instance.GetCoins();
        _currencyText.text = currency.ToString();
    }
}
