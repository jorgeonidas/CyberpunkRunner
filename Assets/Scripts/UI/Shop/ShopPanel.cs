using System;
using TMPro;
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
    [SerializeField] TextMeshProUGUI _priceText;
    [SerializeField] TextMeshProUGUI _purchaseText;
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
        //TODO: expand this to more shop tipes in next updates
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
        bool isAffordable = IsItemAffordable(_selectedStoreItem);
        if (_purchaseButton.gameObject.activeSelf)
        {
            _purchaseButton.interactable = isAffordable;
            _priceText.text = _selectedStoreItem.Price.ToString();
            _priceText.color = isAffordable ? Color.green : Color.red;
            _purchaseText.color = isAffordable ? Color.white : Color.red;
        }

        bool isEquipped = UserDataServiceSO.Instance.IsEquipped(category, itemId);
        _equipButton.gameObject.SetActive(owned && !isEquipped);
        _equippedLabel.gameObject.SetActive(owned && isEquipped);
        _priceText.gameObject.SetActive(!owned);
        
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

    public void OnPurchaseButtonPressed()
    {
        if (_selectedStoreItem == null)
        {
            Debug.LogError($"No item selected");
            return;
        }


    }

    public bool IsItemAffordable(StoreItemSO product)
    {
        return product.Price <= UserDataServiceSO.Instance.GetCoins();
    }
}
