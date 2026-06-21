using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Minigame : MonoBehaviour, IInteractable
{
    [Header("Quest")]
    public string questName;
    public bool questActive;

    [Header("Minigame")]
    public string minigameSceneString;
    private bool isDialogueActive;
    private DialogueController dialogueUI; // Used to show player options to start game
    private GameObject interactionIndicator;

    private void Start()
    {
        dialogueUI = DialogueController.instance;
        interactionIndicator = transform.GetChild(0).gameObject;
        
        if(QuestController.instance.GetQuestStatus(questName) == QuestStatus.Active)
            questActive = true;
    }

    private void Update()
    {
        interactionIndicator.SetActive(CanInteract());
    }

    public bool CanInteract()
    {
        return questActive && !isDialogueActive;
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