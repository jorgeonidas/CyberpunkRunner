using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : AbstractUIPanel
{
    public override string Id => StringConstants.MainMenuPanels.ShopMenu;
    [Header("Shop Elements")]
    [SerializeField] Button _equipButton;
    [SerializeField] GameObject _equippedLabel;
    [SerializeField] Button _purchaseButton;
    [SerializeField] ShopView _paintShop;
    StoreCatalog _catalog => StoreCatalog.Instance;
    StoreItemSO _selectedStoreItem;
    
    private void OnEnable()
    {
        UserDataServiceSO.Instance.OnEquippedChanged += OnItemEquipped;
    }

    private void OnDisable()
    {
        UserDataServiceSO.Instance.OnEquippedChanged -= OnItemEquipped;    
    }

    public override void Show()
    {
        _paintShop.InitializeShopItems(_catalog, ItemSelected);
        base.Show();
    }

    private void ItemSelected(string itemId, ProductCategory category)
    {
        Debug.Log($"itemId {itemId} category {category}");
        HandleSelectedItemState(category, itemId);
        UserDataServiceSO.Instance.Preview(category, itemId);
    }

    private void HandleSelectedItemState(ProductCategory category, string itemId)
    {
        _selectedStoreItem = _catalog.GetById(itemId);
        bool owned = UserDataServiceSO.Instance.Owns(category, itemId);
        _purchaseButton.gameObject.SetActive(!owned);

        bool isEquipped = UserDataServiceSO.Instance.IsEquipped(category, itemId);
        _equipButton.gameObject.SetActive(owned && !isEquipped);
        _equippedLabel.gameObject.SetActive(owned && isEquipped);
    }

    private void OnItemEquipped(ProductCategory category, string arg2)
    {
        HandleSelectedItemState(category, _selectedStoreItem.Id);
        _paintShop.RefreshProductsList();
    }

    public void OnEquiButtonPressed()
    {
        Debug.Log($"Try equip {_selectedStoreItem.Id}");
        UserDataServiceSO.Instance.Equip(_selectedStoreItem.Category, _selectedStoreItem.Id);
    }
}
