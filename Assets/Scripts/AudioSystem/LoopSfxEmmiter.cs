using UnityEngine;

public class LoopSfxEmmiter : MonoBehaviour
{
    [SerializeField] private SfxIdEnum.loopSfxId sfxId;
    private Vector3 _lastPosition;
    int _instanceId;
    private void Start()
    {
        _instanceId = gameObject.GetInstanceID();
        _lastPosition = transform.position;
    }

    private void Update()
    {
        if (transform.position != _lastPosition)
        {
            UpdateLoopSfxPosition();
            _lastPosition = transform.position;
        }
    }


    public void PlayLoopSfx()
    {
        if (sfxId != SfxIdEnum.loopSfxId.None)
        {
            SfxManager.Instance.PlayLoopSfx(sfxId, transform.position, _instanceId);
        }
    }

    public void UpdateLoopSfxPosition()
    {
        SfxManager.Instance.UpdateLoopSfxPosition(_instanceId, transform.position);
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
