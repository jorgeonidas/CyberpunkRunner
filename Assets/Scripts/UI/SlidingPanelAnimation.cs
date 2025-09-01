using UnityEngine;
using DG.Tweening;
using System;

public class SlidingPanelAnimation : MonoBehaviour
{
    public Action OnOpen;
    public Action OnClosed;
    [SerializeField] private RectTransform _rectTransform;
    [SerializeField] float _positionOffset = 330;
    [SerializeField] float _animationLength = 0.25f;
    Sequence _sequence;
    public void Open()
    {
        _rectTransform.anchoredPosition = new Vector2(_positionOffset, _rectTransform.anchoredPosition.y);
        if (_sequence != null && _sequence.IsActive())
        {
            _sequence.Kill();
        }

        _sequence = DOTween.Sequence();
        _sequence.Append(_rectTransform.DOAnchorPosX(-_positionOffset, _animationLength)).OnComplete(() =>
        {
            OnOpen?.Invoke();
        });
    }

    public void Close()
    {
        if (_sequence != null && _sequence.IsActive())
        {
            _sequence.Kill();
        }

        _sequence = DOTween.Sequence();
        _sequence.Append(_rectTransform.DOAnchorPosX(_positionOffset, _animationLength)).OnComplete(() =>
        {
            OnClosed?.Invoke();
        });
    }

}
