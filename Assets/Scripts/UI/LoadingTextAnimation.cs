using TMPro;
using UnityEngine;

public class LoadingTextAnimation : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _loadingText;
    [SerializeField] private float _loadingDotsSpeed = 3f;
    [SerializeField] private float _loadingDotsMax = 4f;

    private void Start()
    {
        _loadingText.text = "Loading";
    }

    private void Update()
    {
        float dotsCount = Mathf.PingPong(Time.time * _loadingDotsSpeed, _loadingDotsMax);
        _loadingText.text = "Loading" + new string('.', (int)dotsCount);
    }
}
