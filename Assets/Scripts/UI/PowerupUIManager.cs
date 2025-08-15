using UnityEngine;
using System.Collections.Generic;

public class PowerupUIManager : MonoBehaviour
{
    [SerializeField] private Transform powerupsDisplayerUI; // Referencia al contenedor PowerupsDisplayerUI
    [SerializeField] private GameObject powerupItemPrefab; // Prefab de PowerupDisplayerUIItem

    private Dictionary<string, PowerupUIItem> activePowerups = new Dictionary<string, PowerupUIItem>();

    public void ActivatePowerup(PowerupBase powerup)
    {
        if (activePowerups.TryGetValue(powerup.Id, out PowerupUIItem existingItem))
        {
            // Si el powerup ya está activo, añade tiempo al progreso
            existingItem.AddTime(powerup.Duration);
        }
        else
        {
            // Si el powerup no está activo, crea un nuevo item en la UI
            GameObject newItem = Instantiate(powerupItemPrefab, powerupsDisplayerUI);
            PowerupUIItem powerupUIItem = newItem.GetComponent<PowerupUIItem>();
            powerupUIItem.Initialize(powerup, this);
            activePowerups.Add(powerup.Id, powerupUIItem);
        }
    }

    public void RemovePowerup(string powerupId)
    {
        if (activePowerups.TryGetValue(powerupId, out PowerupUIItem item))
        {
            Destroy(item.gameObject);//Ill add a pooling system
            activePowerups.Remove(powerupId);
        }
    }
}
