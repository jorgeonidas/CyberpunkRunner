using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerController _playerController;

    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
    }

    public void Initialize()
    {
        
    }
}
