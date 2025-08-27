using UnityEngine;

[CreateAssetMenu(fileName = "StoreItemSO", menuName = "Ingame Store/StoreItemSO")]
public class StoreItemSO : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private string _displayNameKey;
    [SerializeField] private ProductCategory _category;
    [SerializeField] private int _price;
    // [SerializeField] private string _rarity;
    [SerializeField] private Sprite _thumbnail; 

    public string Id => _id;
    public string DisplayNameKey => _displayNameKey;
    public ProductCategory Category => _category;
    public int Price => _price;
    // public string Rarity => _rarity;
    public Sprite Thumbnail => _thumbnail;
}
