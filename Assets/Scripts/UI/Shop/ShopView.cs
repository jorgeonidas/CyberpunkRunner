using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    public Action<string, ProductCategory> OnItemSelected;
    [SerializeField] StoreCatalog _catalog;
    [SerializeField] ProductCategory _productCategory;
    [SerializeField] ShopViewItem _shopViewItemPrefab;
    [SerializeField] Transform _itemsContainer;
    private IEnumerable<StoreItemSO> _products;
    private List<ShopViewItem> _shopItems;

    public void InitializeShopItems(Action<string, ProductCategory> onItemSelected)
    {
        if (_products == null)
        {
            _shopItems = new List<ShopViewItem>();
            _products = _catalog.GetByCategory(_productCategory);
            foreach (StoreItemSO item in _products)
            {
                var newItem = Instantiate(_shopViewItemPrefab, _itemsContainer);
                newItem.Initialize(item, ItemSelected);
                _shopItems.Add(newItem);
            }
        }
        OnItemSelected = onItemSelected;
    }

    private void ItemSelected(string productId, ProductCategory category)
    {
        foreach (ShopViewItem item in _shopItems)
        {
            item.SetSelected(productId == item.ProductId);
        }

        //testing: apply paint to bike

        //todo: 
        //check if owned to show the equip button
        //if is equipped show the button unequip and display the default material
        //if not owner showthe purchase button
        OnItemSelected?.Invoke(productId, category);
    }
}
