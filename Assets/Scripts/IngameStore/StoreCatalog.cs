using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreCatalog", menuName = "Ingame Store/StoreCatalog")]
public class StoreCatalog : SingletonScriptableObject<StoreCatalog>
{
    [SerializeField] private SerializedDictionary<ProductCategory, List<StoreItemSO>> _storeCatalogDictionary = new();

    public StoreItemSO GetItemById(ProductCategory category, string id)
    {
        if (_storeCatalogDictionary.ContainsKey(category))
        {
            List<StoreItemSO> items = _storeCatalogDictionary[category];
            return items.FirstOrDefault(i => i.Id == id);
        }
        return null;
    }

    public IEnumerable<StoreItemSO> GetItemsByCategory(ProductCategory category)
    {
        if (_storeCatalogDictionary.ContainsKey(category))
        {
            return _storeCatalogDictionary[category];
        }
        return null;
    }

    // [SerializeField] private List<StoreItemSO> _items = new();
    // private Dictionary<string, StoreItemSO> _byId;

    // public void Init()
    // {
    //     _byId = _items.ToDictionary(i => i.Id, i => i);
    // }

    // public IEnumerable<StoreItemSO> All => _items;
    // public StoreItemSO GetById(string id) => _byId[id];
    // public IEnumerable<StoreItemSO> GetByCategory(ProductCategory cat) =>
    //     _items.Where(i => i.Category == cat);

    // public StoreItemSO GetStoreItemSO(ProductCategory cat, string id)
    // {
    //     return GetByCategory(cat).FirstOrDefault(i => i.Id == id);
    // }
}
