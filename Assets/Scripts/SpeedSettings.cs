using UnityEngine;

[CreateAssetMenu(fileName = "SpeedSettings", menuName = "Sci-FiRunner/SpeedSettings")]
public class SpeedSettings : ScriptableObject
{
    [Header("Level Speeds")]
    [SerializeField] float _initialChunkSpeed = 14f;
    [Header("Moving objects speeds")]
    [SerializeField] float _initialObjectsSpeed = 20f;
    [Header("Acceleration/Deceleration")]
    [SerializeField] float _acceleration = 14f;
    [SerializeField] float _deceleration = 16f;
    [Header("Side Moving Speed")]
    [SerializeField] float _playerSideMovingSpeed = 8f;

    public float InitialChunkSpeed => _initialChunkSpeed;
    public float InitialObjectsSpeed => _initialObjectsSpeed;
    public float Acceleration => _acceleration;
    public float Deceleration => _deceleration;
    public float PlayerSideMovingSpeed => _playerSideMovingSpeed;
}
