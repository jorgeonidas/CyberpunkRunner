using UnityEngine;
using System.IO;

[System.Serializable]
public class UserData
{
    public int recordDistance;
    public int coinsCollected;
}

public static class SaveSystem
{
    private static string filePath = Application.persistentDataPath + "/userdata.json";

    public static void SaveUserData(UserData userData)
    {
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(filePath, json);
        Debug.Log("Datos guardados en: " + filePath);
    }

    public static UserData LoadUserData()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            UserData userData = JsonUtility.FromJson<UserData>(json);
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
