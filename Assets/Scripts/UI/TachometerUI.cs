using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TachometerUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private RectTransform needleTransform;
    [SerializeField] private Image _fillImage;
    [SerializeField] private Gradient _colorGradient;
    [SerializeField] private TextMeshProUGUI _currentSpeedText;

    [Header("Speed Settings")]
    [SerializeField] private float _smoothSpeed = 5f;
    [SerializeField] private SpeedManager _speedManager;
    [Header("Needle Settings")]
    [SerializeField] private float _minAngle = -90f; // Ángulo mínimo de la aguja
    [SerializeField] private float _maxAngle = 90f;  // Ángulo máximo de la aguja
    [SerializeField] private float _maxFillImageAmount = 0.7f; // Máximo valor de la imagen de relleno
    private float currentSpeed;
    private float _maxSpeed = 100f;

    private void Start()
    {
        if (_speedManager != null)
        {
            _maxSpeed = _speedManager.MaxChunkSpeed;
        }
    }

    private void Update()
    {
        UpdateSpeed();
    }

    private void UpdateSpeed()
    {
        float targetSpeed = _speedManager?.CurrentChunksMoveSpeed ?? 0;

        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, _smoothSpeed * Time.deltaTime);

        float normalizedSpeed = Mathf.Clamp01(currentSpeed / _maxSpeed);

        float targetAngle = Mathf.Lerp(_minAngle, _maxAngle, normalizedSpeed);
        float targerFill = Mathf.Lerp(0, _maxFillImageAmount, normalizedSpeed);

        _fillImage.fillAmount = targerFill;
        needleTransform.localRotation = Quaternion.Euler(0f, 0f, targetAngle);
        _currentSpeedText.text = $"{(currentSpeed * 10).ToString("f0")}\n<size=26>KM/H</size>";
        _currentSpeedText.color = _colorGradient.Evaluate(normalizedSpeed);
        _fillImage.color = _colorGradient.Evaluate(normalizedSpeed);
    }
}