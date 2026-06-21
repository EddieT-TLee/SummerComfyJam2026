using UnityEngine;
using UnityEngine.SceneManagement;

public class ExplodeEverything : MonoBehaviour
{
    void Awake()
    {
        GameObject temp = new GameObject("TempSceneFinder");
        DontDestroyOnLoad(temp);
        Scene ddolScene = temp.scene;
        DestroyImmediate(temp);

        foreach (GameObject obj in ddolScene.GetRootGameObjects())
        {
            DestroyImmediate(obj);
        }
    }
}
