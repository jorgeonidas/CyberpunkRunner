using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPopup : AbstractUIPanel
{
    [SerializeField] private TextMeshProUGUI _tittleText;
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private TextMeshProUGUI _okText;
    [SerializeField] private TextMeshProUGUI _cancelText;
    [SerializeField] private Button _confirmButton;
    [SerializeField] private Button _cancelButton;

    public override string Id => "ConfirmationPopup";

    public void Setup(string title,
                        string message,
                        string okText = "OK",
                        Action onConfirm = null,
                        bool cancelButton = false,
                        string cancelText = "Cancel",
                        Action onCancel = null)
    {
        _tittleText.text = title;
        _messageText.text = message;
        _okText.text = okText;
        
        _confirmButton.onClick.AddListener(() =>
        {
            onConfirm?.Invoke();
            Hide();
        });

        _cancelButton.gameObject.SetActive(cancelButton);
        if (cancelButton)
        {
            _cancelText.text = cancelText;
            _cancelButton.onClick.AddListener(() =>
            {
                onCancel?.Invoke();
                Hide();
            });
        }

        Show();
    }
}
