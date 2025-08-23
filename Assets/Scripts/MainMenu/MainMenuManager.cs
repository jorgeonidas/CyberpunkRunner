using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private SpeedManager _speedManager;
    [SerializeField] private LevelGenerator _levelGenerator;

    public void Start()
    {
        _levelGenerator.Initialize(_speedManager);
    }
}
