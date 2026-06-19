using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minigame : MonoBehaviour, IInteractable
{
    public string minigameSceneString;
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

    private void OpenMinigame()
    {
        SceneLoader.instance.SwitchToNewAdditiveScene(minigameSceneString);

        dialogueUI.ShowDialogueUI(false);
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
}