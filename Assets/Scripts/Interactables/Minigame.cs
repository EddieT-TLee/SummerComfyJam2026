using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minigame : MonoBehaviour, IInteractable
{
    public string minigameScene;
    private bool isDialogueActive;
    private DialogueController dialogueUI; // Used to show player options to start game


    private void Start()
    {
        dialogueUI = DialogueController.instance;
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }

    public void Interact()
    {
        isDialogueActive = true;
        PauseManager.IsPaused = isDialogueActive;

        dialogueUI.ShowDialogueUI(true);
        dialogueUI.SetMingameInfo(gameObject.name);
        dialogueUI.SetDialogueText("Would You like to play " + gameObject.name);

        // Create yes and no Buttons
        dialogueUI.CreateChoiceButton("Yes", () => OpenMinigame());
        dialogueUI.CreateChoiceButton("No", () => CloseDialogue());
    }

    void OpenMinigame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(minigameScene);
        dialogueUI.ClearChoices();
    }

    private void CloseDialogue()
    {
        dialogueUI.ShowDialogueUI(false);
        isDialogueActive = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ClearChoices();
        PauseManager.IsPaused = isDialogueActive;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        dialogueUI.ShowDialogueUI(false);
        
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            // Literally just to make sure the player in not in frame
            playerObj.transform.position = new Vector3(-100, 0, 0);
            PauseManager.IsPaused = true;
        }
    }
}