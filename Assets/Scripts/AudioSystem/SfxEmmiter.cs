using UnityEngine;

public class SfxEmmiter : MonoBehaviour
{
    [SerializeField] private SfxIdEnum.SfxId sfxId;

    public void PlaySfx()
    {
        if (sfxId != SfxIdEnum.SfxId.None)
        {
            SfxManager.Instance.PlaySfx(sfxId);
        }
    }
}
