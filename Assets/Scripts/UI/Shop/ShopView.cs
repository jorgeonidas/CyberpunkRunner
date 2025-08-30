using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    public Action<ProductCategory, string> OnItemSelected;
    [SerializeField] ProductCategory _productCategory;
    [SerializeField] ShopViewItem _shopViewItemPrefab;
    [SerializeField] Transform _itemsContainer;
    private IEnumerable<StoreItemSO> _products;
    private List<ShopViewItem> _shopItems;
    StoreCatalog _catalog;

    public void InitializeShopItems(StoreCatalog catalog, Action<ProductCategory, string> onItemSelected)
    {
        _catalog = catalog;
        if (_products == null)
        {
            _shopItems = new List<ShopViewItem>();
            _products = _catalog.GetByCategory(_productCategory);
            foreach (StoreItemSO item in _products)
            {
                var newItem = Instantiate(_shopViewItemPrefab, _itemsContainer);
                bool owned = UserDataServiceSO.Instance.Owns(_productCategory, item.Id);
                bool equipped = UserDataServiceSO.Instance.IsEquipped(_productCategory, item.Id);
                Debug.Log($"_productCategory {_productCategory} item.Id {item.Id} owned {owned} equipped {equipped}");
                newItem.Initialize(item, ItemSelected, owned, equipped);
                newItem.SetSelected(owned && equipped);
                if (owned && equipped)
                {
                    onItemSelected?.Invoke(_productCategory, item.Id);
                }
                _shopItems.Add(newItem);
            }
        }
        OnItemSelected = onItemSelected;
    }

    public void RefreshProductsList()
    {
        foreach (ShopViewItem item in _shopItems)
        {
            bool owned = UserDataServiceSO.Instance.Owns(_productCategory, item.ProductId);
            bool equipped = UserDataServiceSO.Instance.IsEquipped(_productCategory, item.ProductId);
            item.RefreshBadges(owned, equipped);
        }
    }

    private void ItemSelected(string productId, ProductCategory category)
    {
        foreach (ShopViewItem item in _shopItems)
        {
            item.SetSelected(productId == item.ProductId);
        }

        //testing: apply paint to bike
        OnItemSelected?.Invoke(category, productId);
    }
}
