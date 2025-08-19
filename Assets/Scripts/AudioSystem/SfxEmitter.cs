using UnityEngine;

public class SfxEmitter : MonoBehaviour
{
    [SerializeField] private SfxIdEnum.SfxId sfxId;

    public void PlaySfx()
    {
        if (sfxId != SfxIdEnum.SfxId.None)
        {
            SfxManager.Instance.PlaySfx(sfxId, transform.position);
        }
    }
}
