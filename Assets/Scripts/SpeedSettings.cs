using UnityEngine;

[CreateAssetMenu(fileName = "SpeedSettings", menuName = "Sci-FiRunner/SpeedSettings")]
public class SpeedSettings : ScriptableObject
{
    [Header("Level Speeds")]
    [SerializeField] float _initialChunkSpeed = 14f;
    [SerializeField] float _maxChunkSpeed = 20;
    [Header("Moving objects speeds")]
    [SerializeField] float _initialObjectsSpeed = 20f;
    [SerializeField] float _maxObjectsSpeed = 26f;
    [Header("Acceleration/Deceleration")]
    [SerializeField] float _acceleration = 14f;
    [SerializeField] float _deceleration = 16f;
    [Header("Side Moving Speed")]
    [SerializeField] float _playerSideMovingSpeed = 8f;
    [Header("Increasae after every chunk counts")]
    [SerializeField] int _chunkSpeedIncreaseCycle = 5;
    [SerializeField] float _speedDifficultyIncrementPerCycle = 5;


    public float InitialChunkSpeed => _initialChunkSpeed;
    public float MaxChunkSpeed => _maxChunkSpeed;
    public float InitialObjectsSpeed => _initialObjectsSpeed;
    public float MaxObjectsSpeed => _maxObjectsSpeed;
    public float Acceleration => _acceleration;
    public float Deceleration => _deceleration;
    public float PlayerSideMovingSpeed => _playerSideMovingSpeed;
    public int ChunkSpeedIncreaseCycle => _chunkSpeedIncreaseCycle;
    public float SpeedDifficultyIncrementPerCycle => _speedDifficultyIncrementPerCycle;
}
