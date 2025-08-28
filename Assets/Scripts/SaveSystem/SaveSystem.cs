using UnityEngine;
using System.IO;
using Newtonsoft.Json;


public static class SaveSystem
{
    private static string filePath = Application.persistentDataPath + "/userdata.json";

    public static void SaveUserData(UserData userData)
    {
        string json = JsonConvert.SerializeObject(userData, Formatting.Indented);
        File.WriteAllText(filePath, json);
        Debug.Log("Datos guardados en: " + filePath);
    }

    public static UserData LoadUserData()
    {
        Debug.Log($"Cargando datos desde: {filePath}");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);

            // Deserializar
            UserData userData = JsonConvert.DeserializeObject<UserData>(json);

            if (userData == null)
            {
                Debug.LogWarning("Error al deserializar, creando datos nuevos.");
                return new UserData();
            }

            Debug.Log("Datos cargados desde: " + filePath);
            return userData;
        }
        else
        {
            Debug.LogWarning("Archivo de datos no encontrado. Retornando datos por defecto.");
            return new UserData();
        }
    }
}
