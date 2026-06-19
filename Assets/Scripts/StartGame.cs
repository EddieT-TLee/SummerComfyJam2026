using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartGame : MonoBehaviour
{
    [SerializeField]
    private Button startButton;

    private void Awake() {
        startButton.onClick.AddListener(GoToStartScene);
    }

    private void GoToStartScene()
    {
        SceneManager.LoadScene("Beach");
    }
}
