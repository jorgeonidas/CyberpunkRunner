using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public static class SaveToJson
{
    public static void Save<T>(string fileName, T data, JsonSerializerSettings settings = null)
    {
        var json = JsonConvert.SerializeObject(data, settings ?? new JsonSerializerSettings { Formatting = Formatting.Indented });

#if UNITY_WEBGL && !UNITY_EDITOR
        // In WebGL, save JSON string into PlayerPrefs (persists in IndexedDB)
        PlayerPrefs.SetString(fileName, json);
        PlayerPrefs.Save();
        Debug.Log($"[Save] PlayerPrefs key: {fileName}");
#else
        // On desktop/mobile, save to file
        var path = Path.Combine(Application.persistentDataPath, fileName);
        File.WriteAllText(path, json);
        Debug.Log($"[Save] {path}");
#endif
    }

    public static T Load<T>(string fileName, JsonSerializerSettings settings = null) where T : class
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // In WebGL, load JSON from PlayerPrefs
        if (!PlayerPrefs.HasKey(fileName)) return null;
        var json = PlayerPrefs.GetString(fileName);
        return JsonConvert.DeserializeObject<T>(json, settings);
#else
        // On desktop/mobile, load from file
        var path = Path.Combine(Application.persistentDataPath, fileName);
        //Debug.Log($"{path}");
        if (!File.Exists(path)) return null;
        var json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json, settings);
#endif
    }
}
