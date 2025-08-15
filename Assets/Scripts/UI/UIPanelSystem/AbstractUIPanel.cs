using UnityEngine;

public abstract class AbstractUIPanel : MonoBehaviour, IUIPanel
{
    public virtual string Id => string.Empty;

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
}
