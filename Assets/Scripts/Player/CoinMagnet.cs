using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private float magnetForce = 20f;
    private bool magnetActive = false;

    public bool MagnetActive
    {
        get => magnetActive;
        set => magnetActive = value;
    }

    private void Update()
    {
        if (!magnetActive)
        {
            return;
        }
        AttractCoins();
    }

    private void AttractCoins()
    {
        if (CoinsManager.Instance == null)
        {
            return;
        }
        List<CoinPickUp> coins = CoinsManager.Instance.GetActiveCoins();
        Vector3 magnetPosition = transform.position;
        foreach (var coin in coins)
        {
            if (coin == null)
            {
                continue;
            }
            float dist = Vector3.Distance(coin.transform.position, magnetPosition);
            if (dist <= magnetRadius)
            {
                // Move coin towards the magnet
                Vector3 directionToMagnetSoruce = (magnetPosition - coin.transform.position).normalized;
                coin.transform.position += directionToMagnetSoruce * magnetForce * Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}
