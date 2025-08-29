using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesManager : MonoBehaviour
{
    private const string MAIN_MENU = "MainMenuScene";
    private const string GAME_SCENE = "MainLevel";
    public static ScenesManager Instance { get; private set; }
    [SerializeField] CanvasFader _fadder;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static void ToMainMenu()
    {
        LoadSceneAsync(MAIN_MENU);
    }

    public static void ToGameScene()
    {
        LoadSceneAsync(GAME_SCENE);
    }

    private static void LoadSceneAsync(string sceneName)
    {

        Instance._fadder.FadeIn(() =>
                {
                    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
                    asyncLoad.completed += OnSceneLoaded;
                });
    }

    private static void OnSceneLoaded(AsyncOperation operation)
    {

        Instance._fadder.FadeOut();
    }
}
