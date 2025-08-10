using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReload : MonoBehaviour
{
    // Call this method to reload the current scene
    public void ReloadCurrentScene()
    {
        // Get the currently active scene
        Scene currentScene = SceneManager.GetActiveScene();

        // Load the scene again using its name or build index
        // Using the name is generally more robust as build indices can change.
        SceneManager.LoadScene(currentScene.name); 
    }
}
