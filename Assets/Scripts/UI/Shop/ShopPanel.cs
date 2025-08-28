using System;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : AbstractUIPanel
{
    public static Action<string, ProductCategory> OnItemSelected;
    public override string Id => StringConstants.MainMenuPanels.ShopMenu;
    [Header("Shop Elements")]
    [SerializeField] Button _equipButton;
    [SerializeField] GameObject _equippedLabel;
    [SerializeField] Button _purchaseButton;
    //[SerializeField] Ted _purchaseButton;
    [SerializeField] ShopView _paintShop;

    public override void Show()
    {
        _paintShop.InitializeShopItems(ItemSelected);
        base.Show();
    }

    private void ItemSelected(string itemId, ProductCategory category)
    {
        Debug.Log($"itemId {itemId} category {category}");
        OnItemSelected?.Invoke(itemId, category );
    }
}
