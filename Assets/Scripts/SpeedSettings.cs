using UnityEngine;

[CreateAssetMenu(fileName = "SpeedSettings", menuName = "Sci-FiRunner/SpeedSettings")]
public class SpeedSettings : ScriptableObject
{
    [Header("Level Speeds")]
    [SerializeField] float _initialChunkSpeed = 14;
    [Header("Moving objects speeds")]
    [SerializeField] float _initialObjectsSpeed = 20;
    [Header("Acceleration/Deceleration")]
    [SerializeField] float _acceleration;
    [SerializeField] float _deceleration;
    
    public float InitialChunkSpeed => _initialChunkSpeed;
    public float InitialObjectsSpeed => _initialObjectsSpeed;
    public float Acceleration => _acceleration; 
    public float Deceleration => _deceleration; 
}
