using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    [SerializeField] StoreCatalog _catalog;
    [SerializeField] ProductCategory _productCategory;
    [SerializeField] ShopViewItem _shopViewItemPrefab;
    [SerializeField] Transform _itemsContainer;
    private IEnumerable<StoreItemSO> _products;
    private List<ShopViewItem> _shopItems;


    public void InitializeShopItems()
    {
        if (_products == null)
        {
            _shopItems = new List<ShopViewItem>();
            _products = _catalog.GetByCategory(_productCategory);
            foreach (StoreItemSO item in _products)
            {
                var newItem = Instantiate(_shopViewItemPrefab, _itemsContainer);
                newItem.Initialize(item);
                _shopItems.Add(newItem);
            }
        }
    }
}
