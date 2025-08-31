using System.Linq;
using UnityEngine;
using System;

public class PlayerBikePainter : AbstractInventorySlot
{
    [Header("Bike Mesh filter")]
    [SerializeField] MeshRenderer _meshRenderer;
    PaintItemSO _defaultPaint;
    PaintItemSO _equippedItem;


    public override ProductCategory ProductCategory => ProductCategory.Paint;
    private void Start()
    {
        _defaultPaint = StoreCatalog.Instance.GetItemById(ProductCategory, StringConstants.DefaultItem) as PaintItemSO;
    }

    public void ApplyPaint(Material material)
    {
        _meshRenderer.material = material;
    }

    public override void Equip(StoreItemSO storeItemSO)
    {
        var paintProduct = storeItemSO as PaintItemSO;
        ApplyPaint(paintProduct.PaintMaterial);
        _equippedItem = paintProduct;
    }

    public override void Preview(StoreItemSO storeItemSO)
    {
        var paintProduct = storeItemSO as PaintItemSO;
        ApplyPaint(paintProduct.PaintMaterial);
    }

    public override void Unequip()
    {
        Equip(_defaultPaint);
    }
}
