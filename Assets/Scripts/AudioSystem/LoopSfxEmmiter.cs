using UnityEngine;
using static SfxIdEnum;

public class LoopSfxEmmiter : MonoBehaviour
{
    [SerializeField] private LoopSfxId sfxId;
    private Vector3 _lastPosition;
    int _instanceId;
    private void Start()
    {
        _lastPosition = transform.position;
    }

    public void PlayLoopSfx()
    {
        if (sfxId != LoopSfxId.None)
        {
            _instanceId = gameObject.GetInstanceID();
            SfxManager.Instance.PlayLoopSfx(sfxId, transform, _instanceId);
        }
    }

    public void SetLoopPitch(float pitch)
    {
        SfxManager.Instance.SetLoopSfxPitch(_instanceId, pitch);
    }

    public void StopLoopSfx()
    {
        SfxManager.Instance.StopLoopSfx(_instanceId);
    }
}
