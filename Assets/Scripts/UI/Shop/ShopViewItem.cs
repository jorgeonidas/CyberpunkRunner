using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShopViewItem : MonoBehaviour
{
    [SerializeField] Image _itemIcon;
    [SerializeField] GameObject _ownedBadge;
    [SerializeField] GameObject _equippedBadge;

    public void Initialize(StoreItemSO _storeItem)
    {
        _itemIcon.sprite = _storeItem.Thumbnail;
    }
}
