using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance {get; private set;}

    private bool hasHiddenScene = false;
    private Scene hiddenScene;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Loads a scene additively while hiding the current scene
    /// </summary>
    /// <param name="newSceneName">Scene to load additively</param>
    public void SwitchToNewAdditiveScene(string newSceneName)
    {
        StartCoroutine(LoadAndHide(newSceneName));
    }

    /// <summary>
    /// Switches back to a previously loaded scene and unloads the current additive scene.
    /// </summary>
    public void ReturnToPreviousScene()
    {
        if (!hasHiddenScene) return;

        Scene additiveScene = SceneManager.GetActiveScene();

        ToggleScene(hiddenScene, true);
        SceneManager.SetActiveScene(hiddenScene);

        SceneManager.UnloadSceneAsync(additiveScene);

        hasHiddenScene = false;
        PauseManager.IsPaused = false;
    }

    private IEnumerator LoadAndHide(string sceneString)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        hiddenScene = currentScene;
        hasHiddenScene = true;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneString, LoadSceneMode.Additive);
        PauseManager.IsPaused = true;

        // Wait for scene to load
        while (!loadOperation.isDone)
        {
            yield return null;
        }

        Scene newScene = SceneManager.GetSceneByName(sceneString);
        SceneManager.SetActiveScene(newScene);
        ToggleScene(currentScene, false);
    }

    private static void ToggleScene(Scene scene, bool state)
    {
        if (scene.isLoaded)
        {
            GameObject root = new GameObject();

            foreach (GameObject obj in scene.GetRootGameObjects()) { 
                obj.SetActive(state);
            }
        }
    }
}
