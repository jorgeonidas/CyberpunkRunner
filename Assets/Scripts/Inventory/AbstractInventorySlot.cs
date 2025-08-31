using UnityEngine;

public abstract class AbstractInventorySlot : MonoBehaviour
{
    public abstract ProductCategory ProductCategory { get; }
    public abstract void Equip(StoreItemSO storeItemSO);
    public abstract void Preview(StoreItemSO storeItemSO);
    public abstract void Revert();
}
