#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public static class PlayModeStartScene
{
    private const string SetupScenePath = "Assets/Scenes/Setup.unity";
    private const string MenuUseSetup = "Tools/Play Mode Start Scene/Use Setup";
    private const string MenuClear    = "Tools/Play Mode Start Scene/Clear";

    [MenuItem(MenuUseSetup)]
    public static void UseSetupScene()
    {
        var setup = AssetDatabase.LoadAssetAtPath<SceneAsset>(SetupScenePath);
        if (setup == null)
        {
            Debug.LogError($"Setup scene not found at {SetupScenePath}");
            return;
        }

        EditorSceneManager.playModeStartScene = setup;
        UpdateMenuChecks();
        Debug.Log("Play Mode Start Scene set to Setup.");
    }

    [MenuItem(MenuClear)]
    public static void ClearStartScene()
    {
        EditorSceneManager.playModeStartScene = null;
        UpdateMenuChecks();
        Debug.Log("Play Mode Start Scene cleared.");
    }

    // Validator for "Use Setup" (always enabled)
    [MenuItem(MenuUseSetup, true)]
    private static bool UseSetupScene_Validate() => true;

    // Validator for "Clear" (enabled only if a start scene is set)
    [MenuItem(MenuClear, true)]
    private static bool ClearStartScene_Validate()
    {
        return EditorSceneManager.playModeStartScene != null;
    }

    [InitializeOnLoadMethod]
    private static void OnEditorLoad()
    {
        // Refresh checkmarks when editor domain reloads
        UpdateMenuChecks();

        // Refresh when scene changes in edit mode
        EditorSceneManager.activeSceneChangedInEditMode += (_, __) => UpdateMenuChecks();

        // Refresh when play mode state changes (enter/exit play)
        EditorApplication.playModeStateChanged += _ => UpdateMenuChecks();
    }

    private static void UpdateMenuChecks()
    {
        bool isSetupSelected =
            EditorSceneManager.playModeStartScene != null &&
            AssetDatabase.GetAssetPath(EditorSceneManager.playModeStartScene) == SetupScenePath;

        // Add checkmark on "Use Setup" if Setup is selected
        Menu.SetChecked(MenuUseSetup, isSetupSelected);

        // "Clear" does not usually have a checkmark, keep it unchecked
        Menu.SetChecked(MenuClear, false);
    }
}
#endif
