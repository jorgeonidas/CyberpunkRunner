using UnityEngine;

[CreateAssetMenu(fileName = "PaintItemSO", menuName = "Ingame Store/PaintItemSO")]
public class PaintItemSO : StoreItemSO
{
    // [SerializeField] private string _materialAddress; // Addressables key del Material
    // public string MaterialAddress => _materialAddress;
    [SerializeField] private Material _paintMaterial;
    public Material PaintMaterial => _paintMaterial;
}
