using UnityEngine;

public interface IDestroy
{
    public bool IsDestroyed { get; } 
    void DestroyMe();
}
