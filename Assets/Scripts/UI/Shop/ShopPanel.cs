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
        UserDataServiceSO.Instance.OnInventoryChanged += ItemSelected;

    }

    private void OnDisable()
    {
        UserDataServiceSO.Instance.OnEquippedChanged -= OnItemEquipped;
        UserDataServiceSO.Instance.OnInventoryChanged -= ItemSelected;
    }

    public override void Show()
    {
        //TODO: expand this to more shop tipes in next updates
        _paintShop.InitializeShopItems(_catalog, ItemSelected);
        base.Show();
    }

    private void ItemSelected(ProductCategory category,string itemId)
    {
        Debug.Log($"itemId {itemId} category {category}");
        HandleSelectedItemState(category, itemId);
        UserDataServiceSO.Instance.Preview(category, itemId);
    }

    private void HandleSelectedItemState(ProductCategory category, string itemId)
    {
        _selectedStoreItem = _catalog.GetItemById(category, itemId);
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

    private void OnItemEquipped(ProductCategory category, string itemId)
    {
        HandleSelectedItemState(category, _selectedStoreItem.Id);
        _paintShop.RefreshProductsList();
    }

    public void OnEquiButtonPressed()
    {
        Debug.Log($"Try equip {_selectedStoreItem.Id}");
        UserDataServiceSO.Instance.Equip(_selectedStoreItem.Category, _selectedStoreItem.Id);
        //TODO: if we get more categorys select sfx by category
        SfxManager.Instance.PlayUISfx(SfxIdEnum.UISfxId.EquipPaint);
    }

    public void OnPurchaseButtonPressed()
    {
        if (_selectedStoreItem == null)
        {
            Debug.LogError($"No item selected");
            return;
        }
        bool isAffordable = IsItemAffordable(_selectedStoreItem);
        if (isAffordable)
        {
            UserDataServiceSO.Instance.AddOwned(_selectedStoreItem.Category, _selectedStoreItem.Id);
            UserDataServiceSO.Instance.AddCoins(-_selectedStoreItem.Price);
            _paintShop.RefreshProductsList();
            SfxManager.Instance.PlayUISfx(SfxIdEnum.UISfxId.Purchased);
        }
    }

    public bool IsItemAffordable(StoreItemSO product)
    {
        return product.Price <= UserDataServiceSO.Instance.GetCoins();
    }
}
