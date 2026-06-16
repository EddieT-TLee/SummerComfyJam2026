using UnityEngine;
using UnityEngine.SceneManagement;

public class Minigame : MonoBehaviour, IInteractable
{
    public string minigameScene;
    private bool isDialogueActive;

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        Debug.Log("Minigame interact");
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(minigameScene);
    }
    
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            // Literally just to make sure the player in not in frame
            playerObj.transform.position = new  Vector3(-100, 0, 0);
            PauseManager.IsPaused = true;
        }
    }
}
