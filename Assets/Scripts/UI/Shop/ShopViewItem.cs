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

    public void Initialize(StoreItemSO storeItem, Action<string, ProductCategory> onItemSelected)
    {
        _storeItem = storeItem;
        _itemIcon.sprite = _storeItem.Thumbnail;
        OnItemSelected = onItemSelected;
    }

    public void RefreshBadges(bool onwed, bool equipped)
    {
        _ownedBadge.SetActive(onwed);
        _equippedBadge.SetActive(equipped);
    }

    public void OnClick()
    {
        OnItemSelected?.Invoke(_storeItem.Id, _storeItem.Category);
    }

    public void SetSelected(bool _isSelected)
    {
        _selectedObject.SetActive(_isSelected);
    }
}
