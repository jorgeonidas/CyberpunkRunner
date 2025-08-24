using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

public static class PopupFactory
{
    private static ConfirmationPopup _popupPrefab;

    private static ConfirmationPopup PopupPrefab
    {
        get
        {
            if (_popupPrefab == null)
            {
                _popupPrefab = Resources.Load<ConfirmationPopup>("UI/ConfirmationPopup"); 
                if (_popupPrefab == null)
                {
                    Debug.LogError("No se encontró el prefab PopupConfirm en Resources/UI/");
                }
            }
            return _popupPrefab;
        }
    }

    private static ConfirmationPopup _currentPopup;

    public static void ShowConfirmationPopup(string title,
                        string message,
                        string okText = "OK",
                        Action onConfirm = null,
                        bool cancelButton = false,
                        string cancelText = "Cancel",
                        Action onCancel = null)
    {
        if (PopupPrefab == null)
        {
            return;
        }

        if (_currentPopup == null)
        {
            _currentPopup = Object.Instantiate(PopupPrefab);
        }

        _currentPopup.Setup(title, message, okText, onConfirm, cancelButton, cancelText, onCancel);
    }
}
