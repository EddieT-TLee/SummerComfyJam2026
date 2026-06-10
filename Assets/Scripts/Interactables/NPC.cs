using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue ScriptableObject")] public NPCDialogue dialogueData;

    private DialogueController dialogueUI;
    private int dialogueIndex;
    private DialogueLine[] dialogueLines;
    private bool isTyping, isDialogueActive, hasChoice;
    private bool reset;
    private Sprite currentPortrait;
    private Sprite[]  portraits;


    private void Start()
    {
        dialogueUI = DialogueController.instance;
        dialogueLines = dialogueData.dialogueLines;
        portraits = dialogueData.npcPortraitSprites;
        
        dialogueIndex = 0;
        reset = false;
    }

    public bool CanInteract()
    {
        return !isDialogueActive;
    }


    public void Interact()
    {
        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        dialogueIndex = 0; 
        currentPortrait = portraits[dialogueLines[dialogueIndex].portraitIndex];
        
        dialogueUI.SetNPCInfo(dialogueData.npcName, currentPortrait);
        dialogueUI.ShowDialogueUI(true);
        DisplayCurrentLine();
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueLines[dialogueIndex].text);
            isTyping = false;
            return;
        }

        //Clear any choices
        dialogueUI.ClearChoices();

        DialogueLine current = dialogueLines[dialogueIndex];

        //Check if choices exist for line and display
        if (current.choices.Length > 0)
        {
            foreach (var choice in current.choices)
                dialogueUI.CreateChoiceButton(choice.choiceText,
                    () => ChooseOption(choice));
            return;
        }


        if (!hasChoice)
        {
            if (++dialogueIndex < dialogueLines.Length)
            {
                DisplayCurrentLine();
            }
            else
            {
                EndDialogue();
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        foreach (char c in dialogueLines[dialogueIndex].text)
        {
            if (dialogueUI.dialogueText.text.Length < 135)
            {
                dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += c);
                yield return new WaitForSeconds(dialogueData.typingSpeed);
            }
        }

        isTyping = false;
    }

    void ChooseOption(DialogueChoice choice)
    {
        dialogueUI.ClearChoices();
        // Convert the array of choice lines into dialogue lines
        dialogueLines = Array.ConvertAll(choice.choiceLines,
            choiceLine => new DialogueLine {text = choiceLine, choices = Array.Empty<DialogueChoice>()});

        if (choice.resetDialogue)
        {
            reset = true;
        }
        dialogueIndex = 0;
        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
        if (reset) // Reset to start of Dialogue data
        {
            dialogueLines = dialogueData.dialogueLines;
            reset = false;
        }
    }
}