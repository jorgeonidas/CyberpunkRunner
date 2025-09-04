using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    [SerializeField] private float magnetRadius = 5f;
    [SerializeField] private float magnetForce = 20f;
    private bool magnetActive = false;
    private float _coinReachDistance = 0.25f;

    // Lista de monedas magnetizadas
    private readonly List<CoinPickUp> magnetizedCoins = new List<CoinPickUp>();

    public bool MagnetActive
    {
        get => magnetActive;
        set => magnetActive = value;
    }

    private void Update()
    {
        if (magnetActive)
        {
            RegisterMagnetizedCoins();
        }
        MoveMagnetizedCoins();
    }

    // Registra monedas dentro del radio como magnetizadas
    private void RegisterMagnetizedCoins()
    {
        if (CoinsManager.Instance == null)
        {
            return;
        }
        List<CoinPickUp> coins = CoinsManager.Instance.GetActiveCoins();
        Vector3 magnetPosition = transform.position;
        foreach (var coin in coins)
        {
            if (coin == null) continue;
            if (magnetizedCoins.Contains(coin)) continue;
            float dist = Vector3.Distance(coin.transform.position, magnetPosition);
            if (dist <= magnetRadius)
            {
                coin.Magnetize(true);
                magnetizedCoins.Add(coin);
            }
        }
    }

    // Mueve todas las monedas magnetizadas hacia el magneto
    private void MoveMagnetizedCoins()
    {
        Vector3 magnetPosition = transform.position;
        // Usar una lista temporal para evitar modificar la colección durante la iteración
        List<CoinPickUp> coinsToRemove = new List<CoinPickUp>();
        foreach (var coin in magnetizedCoins)
        {
            if (coin == null)
            {
                coinsToRemove.Add(coin);
                continue;
            }
            // Si la moneda ya llegó al magneto (puedes ajustar el umbral si es necesario)
            float dist = Vector3.Distance(coin.transform.position, magnetPosition);
            if (dist < _coinReachDistance)
            {
                coinsToRemove.Add(coin);
                continue;
            }
            Vector3 directionToMagnetSource = (magnetPosition - coin.transform.position).normalized;
            coin.transform.position += directionToMagnetSource * magnetForce * Time.deltaTime;
        }
        // Eliminar monedas que ya llegaron o son nulas
        foreach (var coin in coinsToRemove)
        {
            magnetizedCoins.Remove(coin);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, magnetRadius);
    }
}
