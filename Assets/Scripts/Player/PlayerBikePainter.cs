using System.Linq;
using UnityEngine;
using System;

public class PlayerBikePainter : MonoBehaviour
{
    //TODO: for testing we need to pull the store catallog from somehere
    [SerializeField] StoreCatalog _storeCatalog;
    [Header("Bike Mesh filter")]
    [SerializeField] MeshRenderer _meshRenderer;
    PaintItemSO[] availablePaints;
    private string _currentPaintId = StringConstants.DefaultItem;

    private void Start()
    {
        availablePaints = _storeCatalog.GetByCategory(ProductCategory.Paint)
                                      .OfType<PaintItemSO>()
                                      .ToArray();
        ApplyPaint(_currentPaintId);
        ShopPanel.OnItemSelected += OnItemSelected;
    }

    void OnDisable()
    {
        ShopPanel.OnItemSelected -= OnItemSelected;
    }

    public void ApplyPaint(string paintId)
    {
        var currentPaint = availablePaints.Where(x => x.Id == paintId).FirstOrDefault();
        Debug.Log($"paintId {paintId} found? {currentPaint != null}");
        if (currentPaint != null && _meshRenderer != null)
        {
            _meshRenderer.material = currentPaint.PaintMaterial;
        }
    }

    private void OnItemSelected(string productId, ProductCategory category)
    {
        if (category == ProductCategory.Paint)
        {
            ApplyPaint(productId);
        }
    }
}
