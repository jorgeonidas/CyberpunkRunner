using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    public static MainMenuManager Instance { get; private set; }
    [SerializeField] private SpeedManager _speedManager;
    [SerializeField] private LevelGenerator _levelGenerator;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Start()
    {
        _levelGenerator.Initialize(_speedManager);
    }

    public void OnPlayButtonPressed()
    {
        ScenesManager.ToGameScene();
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
