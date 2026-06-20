using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader instance {get; private set;}

    private GameObject player = null;
    private bool hasHiddenScene = false;
    private Scene hiddenScene;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    /// <summary>
    /// Loads a scene additively while hiding the current scene
    /// </summary>
    /// <param name="newSceneName">Scene to load additively</param>
    public void SwitchToNewAdditiveScene(string newSceneName)
    {
        MusicController.instance.StopMusic();
        StartCoroutine(LoadAndHide(newSceneName));
    }

    /// <summary>
    /// Switches back to a previously loaded scene and unloads the current additive scene.
    /// </summary>
    public void ReturnToPreviousScene()
    {
        StartCoroutine(ReturnToScene());
    }

    private IEnumerator ReturnToScene()
    {
        if (!hasHiddenScene) yield break;
        Scene additiveScene = SceneManager.GetActiveScene();

        yield return StartCoroutine(ScreenFader.instance.FadeOut());

        AsyncOperation loadOperation = SceneManager.UnloadSceneAsync(additiveScene);
        PauseManager.IsPaused = false;

        while (!loadOperation.isDone)
        {
            yield return null;
        }
        
        SceneManager.SetActiveScene(hiddenScene);
        ToggleScene(hiddenScene, true);
        hasHiddenScene = false;
        Debug.Log("Start fade out");
        yield return StartCoroutine(ScreenFader.instance.FadeIn());

    }

    private IEnumerator LoadAndHide(string sceneString)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        hiddenScene = currentScene;
        hasHiddenScene = true;

        yield return StartCoroutine(ScreenFader.instance.FadeOut());

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
        
        yield return StartCoroutine(ScreenFader.instance.FadeIn());
    }

    private void ToggleScene(Scene scene, bool state)
    {
        if (scene.isLoaded)
        {
            foreach (GameObject obj in scene.GetRootGameObjects()) { 
                obj.SetActive(state);
            }

            player.SetActive(state);
        }
    }
}
