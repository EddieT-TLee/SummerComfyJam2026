using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue ScriptableObject")]
    public NPCDialogue dialogueData;
   
    private DialogueController dialogueUI;
    private int dialogueIndex;
    private String[] dialogueLines;
    private bool isTyping, isDialogueActive;


    private void Start()
    {
        dialogueUI = DialogueController.instance;
        dialogueLines = dialogueData.dialogueLines;
        dialogueIndex = 0;
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
        
        dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcPortrait);
        dialogueUI.ShowDialogueUI(true);
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueLines[dialogueIndex]);
            isTyping = false;
        } 
        else if (++dialogueIndex < dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");
        
        foreach (char c in dialogueLines[dialogueIndex])
        {
            if(dialogueUI.dialogueText.text.Length < 135)
            {
                dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += c);
                yield return new WaitForSeconds(dialogueData.typingSpeed);
            }
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
    }

}
