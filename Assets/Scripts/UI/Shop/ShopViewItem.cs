using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopViewItem : MonoBehaviour
{
    public Action<string, ProductCategory> OnItemSelected;
    [SerializeField] Image _itemIcon;
    [SerializeField] GameObject _ownedBadge;
    [SerializeField] GameObject _equippedBadge;
    [SerializeField] GameObject _selectedObject;
    StoreItemSO _storeItem;
    public string ProductId => _storeItem.Id;
    private bool _isSelected;
    private bool _owned;
    private bool _equipped;


    public void Initialize(StoreItemSO storeItem, Action<string, ProductCategory> onItemSelected, bool owned, bool equipped)
    {
        _isSelected = false;
        _storeItem = storeItem;
        _itemIcon.sprite = _storeItem.Thumbnail;
        OnItemSelected = onItemSelected;
        RefreshBadges(owned, equipped);
    }

    public void RefreshBadges(bool onwed, bool equipped)
    {
        SetOwned(onwed);
        SetEquipped(equipped);
    }

    public void SetOwned(bool owned)
    {
        _owned = owned;
        _ownedBadge.SetActive(_owned);
    }

    public void SetEquipped(bool equipped)
    {
        _equipped = equipped;
        _equippedBadge.SetActive(_equipped);
    }
    public void OnClick()
    {
        OnItemSelected?.Invoke(_storeItem.Id, _storeItem.Category);
    }

    public void SetSelected(bool isSelected)
    {
        _isSelected = isSelected;
        _selectedObject.SetActive(_isSelected);
    }
}
