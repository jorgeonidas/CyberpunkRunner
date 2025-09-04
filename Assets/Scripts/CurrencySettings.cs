using UnityEngine;

[CreateAssetMenu(fileName = "CurrencySetting", menuName = "Currencys/Currency Setting")]
public class CurrencySetting : ScriptableObject
{
    [SerializeField] int _coinValue = 5;

    public int CoinValue => _coinValue;
}
