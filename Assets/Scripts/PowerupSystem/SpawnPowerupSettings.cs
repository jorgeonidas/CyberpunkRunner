using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class SpawnPowerup
{
    [SerializeField] private PowerupBase _powerupData;
    [Range(0, 1)]
    [SerializeField] private float _dropChance = 0.0f;

    public PowerupBase PowerUp => _powerupData;
    public float DropChance => _dropChance;
}


[CreateAssetMenu(fileName = "SpawnPowerupSettings", menuName = "Powerups Settings/SpawnPowerupSettings")]
public class SpawnPowerupSettings : ScriptableObject
{
    [SerializeField] private List<SpawnPowerup> _powerupList = new List<SpawnPowerup>();

    public PowerupBase ChoosePowerup()
    {
        PowerupBase chosenDrop = null;
        float randomChance = Random.value;
        float probabilityTotal = 0f;

        foreach (SpawnPowerup powerupData in _powerupList)
        {
            chosenDrop = powerupData.PowerUp;
            probabilityTotal += powerupData.DropChance; 
            if (probabilityTotal >= randomChance)
            {
                break; 
            }
        }
        Debug.Log($"<color=green>randomChance {randomChance} probabilityTotal {probabilityTotal} chosenDrop {chosenDrop?.Id}</color>");
        return chosenDrop;

    }
}
