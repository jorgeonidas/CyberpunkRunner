using UnityEngine;

public abstract class AbstractUIPanel : MonoBehaviour, IUIPanel
{
    public abstract string Id { get; }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }

    public virtual void Show()
    {
        gameObject.SetActive(true);
    }
}
