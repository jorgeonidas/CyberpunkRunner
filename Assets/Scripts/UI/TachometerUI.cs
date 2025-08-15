using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TachometerUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Slider _progressBar;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Gradient _colorGradient;
    [SerializeField] private TextMeshProUGUI _currentSpeedText;

    [Header("Speed Settings")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private SpeedManager speedManager;

    private float currentSpeed;
    private float _maxSpeed = 100f;

    private void Start()
    {
        if (speedManager != null)
        {
            _maxSpeed = speedManager.MaxChunkSpeed * 2;
        }
    }

    private void Update()
    {
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        float targetSpeed = speedManager?.CurrentChunksMoveSpeed ?? 0;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, smoothSpeed * Time.deltaTime);

        float fillAmount = Mathf.Clamp01(currentSpeed / _maxSpeed);

        _progressBar.value = fillAmount;

        _fillImage.color = _colorGradient.Evaluate(fillAmount);

        _currentSpeedText.text = $"{(currentSpeed * 10).ToString("f0")} Km/h";
    }
}