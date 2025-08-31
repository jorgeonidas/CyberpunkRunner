using AYellowpaper.SerializedCollections;
using UnityEngine;

public class CosmetisInventory : MonoBehaviour
{
    [SerializeField] SerializedDictionary<ProductCategory, AbstractInventorySlot> _cosmeticsSlots;
    StoreCatalog _storeCatalog => StoreCatalog.Instance;
    UserDataServiceSO _userDataService => UserDataServiceSO.Instance;

    private void Start()
    {
        Initialize();
        _userDataService.OnEquippedChanged += EquipItem;
        _userDataService.OnPreviewItem += PreviewItem;
    }

    void OnDestroy()
    {
        _userDataService.OnEquippedChanged -= EquipItem;
        _userDataService.OnPreviewItem -= PreviewItem;
    }

    public void Initialize()
    {
        foreach (var kvp in _cosmeticsSlots)
        {
            var category = kvp.Key;
            var slot = kvp.Value;
            //get item in category
            var equippedCosmeticId = UserDataServiceSO.Instance.GetEquipped(category);
            EquipItem(category, equippedCosmeticId);
        }
    }
    public void EquipItem(ProductCategory category, string id)
    {
        if (_cosmeticsSlots.ContainsKey(category))
        {
            _cosmeticsSlots[category].Equip(_storeCatalog.GetItemById(category, id));
        }
    }

    public void PreviewItem(ProductCategory category, string id)
    {
        if (_cosmeticsSlots.ContainsKey(category))
        {
            _cosmeticsSlots[category].Preview(_storeCatalog.GetItemById(category, id));
        }
    }
}
