using UnityEngine;

[System.Serializable]
public class UserData
{
    public int recordDistance;
    public int coinsCollected;
    public UserGameSettings userGameSettings;
}

[System.Serializable]
public class UserGameSettings
{
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
}
