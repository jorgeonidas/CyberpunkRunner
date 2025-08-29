using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "StoreCatalog", menuName = "Ingame Store/StoreCatalog")]
public class StoreCatalog : SingletonScriptableObject<StoreCatalog>
{
    [SerializeField] private List<StoreItemSO> _items = new();
    private Dictionary<string, StoreItemSO> _byId;

    public void Init()
    {
        _byId = _items.ToDictionary(i => i.Id, i => i);
    }

    public IEnumerable<StoreItemSO> All => _items;
    public StoreItemSO GetById(string id) => _byId[id];
    public IEnumerable<StoreItemSO> GetByCategory(ProductCategory cat) =>
        _items.Where(i => i.Category == cat);
}
