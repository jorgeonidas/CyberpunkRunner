using UnityEngine;

public class UIPanelsOrganizer : MonoBehaviour, IUIPanelsOrganizer
{
    [Header("Panels Catalog")]
    [SerializeField] protected UIPanelCatalog _panelCatalog;
    void Awake()
    {
        _panelCatalog.Initialize();
    }

    public void Show(string panelId)
    {
        if (_panelCatalog.TryGet(panelId, out IUIPanel panel))
        {
            panel.Show();
        }
    }

    public void Hide(string panelId)
    {
        if (_panelCatalog.TryGet(panelId, out IUIPanel panel))
        {
            panel.Hide();
        }
    }
}
