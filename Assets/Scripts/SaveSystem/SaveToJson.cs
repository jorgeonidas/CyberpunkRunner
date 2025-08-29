using System.IO;
using UnityEngine;
using Newtonsoft.Json;
public static class SaveToJson
{
    public static void Save<T>(string fileName, T data, JsonSerializerSettings settings = null)
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);
        var json = JsonConvert.SerializeObject(data, settings ?? new JsonSerializerSettings { Formatting = Formatting.Indented });
        File.WriteAllText(path, json);
        Debug.Log($"[Save] {path}");
    }

    public static T Load<T>(string fileName, JsonSerializerSettings settings = null) where T : class
    {
        var path = Path.Combine(Application.persistentDataPath, fileName);
        if (!File.Exists(path)) return null;
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json, settings);
    }
}
