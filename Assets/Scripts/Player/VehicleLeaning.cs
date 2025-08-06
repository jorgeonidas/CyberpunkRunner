using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class VehicleLeaning : MonoBehaviour
{
    [SerializeField] Transform _vehicleTransform;
    [SerializeField] float _leaningAmount = 35;
    [SerializeField] float _leaningLength = 1f;
    Sequence _leanSequence;

    public void LeanHorizontal(float horizontalDirection)
    {
        Vector3 currentEulerRotation = transform.rotation.eulerAngles;
        float zRot = -(horizontalDirection * _leaningAmount);
        Vector3 newRotation = new Vector3(currentEulerRotation.x, currentEulerRotation.y, zRot);

        TryKillSequence();
        _leanSequence = DOTween.Sequence();
        _leanSequence.Append(transform.DOLocalRotate(newRotation, _leaningLength, RotateMode.Fast));
    }

    private void TryKillSequence()
    {
        if (_leanSequence != null && _leanSequence.IsActive())
        {
            _leanSequence.Kill();
        }
    }

}
