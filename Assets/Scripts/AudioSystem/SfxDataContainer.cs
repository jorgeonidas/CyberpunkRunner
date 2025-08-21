using AYellowpaper.SerializedCollections;
using UnityEngine;
using static SfxIdEnum;

[CreateAssetMenu(fileName = "SfxDataContainer", menuName = "Game SFX/SfxDataContainer")]
public class SfxDataContainer : ScriptableObject
{
    [Header("Sound Tracks")]
    [SerializeField] private SerializedDictionary<SoundTrackId, SfxData> _soundTrackDictionary;
    [SerializeField] private SerializedDictionary<SfxId, SfxData> _sfxDataDictionary;
    [SerializeField] private SerializedDictionary<LoopSfxId, SfxData> _loopSfxDataDictionary;

    public SfxData GetSfxData(SfxId sfxId)
    {
        if (_sfxDataDictionary.TryGetValue(sfxId, out var sfxData))
        {
            return sfxData;
        }
        Debug.LogWarning($"SFX Data not found for ID: {sfxId}");
        return null;
    }

    public SfxData GetLoopSfxData(LoopSfxId sfxId)
    {
        if (_loopSfxDataDictionary.TryGetValue(sfxId, out var sfxData))
        {
            return sfxData;
        }
        Debug.LogWarning($"Loop SFX Data not found for ID: {sfxId}");
        return null;
    }
    
    public SfxData GetSoundTrackData(SoundTrackId sfxId)
    {
        if (_soundTrackDictionary.TryGetValue(sfxId, out var sfxData))
        {
            return sfxData;
        }
        Debug.LogWarning($"Sound Track Data not found for ID: {sfxId}");
        return null;
    }
}
