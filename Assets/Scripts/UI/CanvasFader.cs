using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
public class CanvasFader : MonoBehaviour
{
    [SerializeField] private CanvasGroup _fadder;
    [SerializeField] private Image _blockImage;
    [SerializeField] private float _fadeDuration = 0.5f;

    private void Start()
    {
        _blockImage.raycastTarget = false;
    }

    public void FadeIn(Action onComplete = null)
    {
        _blockImage.raycastTarget = true;
        _fadder.DOFade(1, _fadeDuration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }

    public void FadeOut(Action onComplete = null)
    {
        _fadder.DOFade(0, _fadeDuration).OnComplete(() =>
        {
            _blockImage.raycastTarget = false;
            onComplete?.Invoke();
        });
    }
}
