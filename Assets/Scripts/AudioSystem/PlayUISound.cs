using UnityEngine;
using static SfxIdEnum;

public class PlayUISound : MonoBehaviour
{
    [SerializeField] UISfxId _uiSfxId;

    public void PlayUISfx()
    {
        SfxManager.Instance.PlayUISfx(_uiSfxId);
    }
}
