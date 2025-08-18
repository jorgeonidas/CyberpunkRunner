using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "SfxDataContainer", menuName = "Game SFX/SfxDataContainer")]
public class SfxDataContainer : ScriptableObject
{
    [SerializeField] private SerializedDictionary<SfxIdEnum.SfxId, SfxData> _sfxDataDictionary;
    [SerializeField] private SerializedDictionary<SfxIdEnum.loopSfxId, SfxData> _loopSfxDataDictionary;

    public SfxData GetSfxData(SfxIdEnum.SfxId sfxId)
    {
        if (_sfxDataDictionary.TryGetValue(sfxId, out var sfxData))
        {
            return sfxData;
        }
        Debug.LogWarning($"SFX Data not found for ID: {sfxId}");
        return null;
    }
    
    public SfxData GetLoopSfxData(SfxIdEnum.loopSfxId sfxId)
    {
        if (_loopSfxDataDictionary.TryGetValue(sfxId, out var sfxData))
        {
            return sfxData;
        }
        Debug.LogWarning($"Loop SFX Data not found for ID: {sfxId}");
        return null;
    }
}
