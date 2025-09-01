using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopView : MonoBehaviour
{
    public Action<ProductCategory, string> OnItemSelected;
    public Action OnViewClosed;
    [SerializeField] ProductCategory _productCategory;
    [SerializeField] ShopViewItem _shopViewItemPrefab;
    [SerializeField] Transform _itemsContainer;
    [SerializeField] SlidingPanelAnimation _slidingAnim;
    private IEnumerable<StoreItemSO> _products;
    private List<ShopViewItem> _shopItems;
    StoreCatalog _catalog;

    private void OnEnable()
    {
        _slidingAnim.OnClosed += ViewClosed;
    }
    
    private void OnDisable()
    {

        _slidingAnim.OnClosed -= ViewClosed;
        UserDataServiceSO.Instance.OnRevertItem?.Invoke(_productCategory);
    }

    public void InitializeShopItems(StoreCatalog catalog, Action<ProductCategory, string> onItemSelected, Action onClosed)
    {
        _catalog = catalog;
        if (_products == null)
        {
            _shopItems = new List<ShopViewItem>();
            _products = _catalog.GetItemsByCategory(_productCategory);
            foreach (StoreItemSO item in _products)
            {
                var newItem = Instantiate(_shopViewItemPrefab, _itemsContainer);
                bool owned = UserDataServiceSO.Instance.Owns(_productCategory, item.Id);
                bool equipped = UserDataServiceSO.Instance.IsEquipped(_productCategory, item.Id);
                newItem.Initialize(item, ItemSelected, owned, equipped);
                _shopItems.Add(newItem);
            }
        }

        foreach (var shopItem in _shopItems)
        {
            bool owned = UserDataServiceSO.Instance.Owns(_productCategory, shopItem.ProductId);
            bool equipped = UserDataServiceSO.Instance.IsEquipped(_productCategory, shopItem.ProductId);
            shopItem.SetSelected(owned && equipped);

            //to refresh shop panel state
            if (equipped)
            {
                onItemSelected?.Invoke(_productCategory, shopItem.ProductId);
            }
        }

        OnItemSelected = onItemSelected;
        _slidingAnim.Open();
        OnViewClosed = onClosed;
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

    
    public void Close()
    {
        _slidingAnim.Close();
    }

    private void ViewClosed()
    {
        OnViewClosed?.Invoke();
    }
}
