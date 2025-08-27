using UnityEngine;

public class ShopPanel : AbstractUIPanel
{
    public override string Id => StringConstants.MainMenuPanels.ShopMenu;
    [SerializeField] ShopView _paintShop;

    public override void Show()
    {
        _paintShop.InitializeShopItems();
        base.Show();
    }
}
