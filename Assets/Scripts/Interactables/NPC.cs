using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Not done
/// </summary>
public class NPC : MonoBehaviour, IInteractable
{
    [Header("Dialogue ScriptableObject")]
    public NPCDialogue dialogueData;
    [Header("Dialogue UI Elements")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    
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

        portraitImage.sprite = dialogueData.npcPortrait;
        nameText.SetText(dialogueData.npcName);
        
        dialoguePanel.SetActive(true);
        StartCoroutine(TypeLine());

    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        } 
        else if (++dialogueIndex < dialogueData.dialogueLines.Length)
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
        dialogueText.SetText("");

        foreach (char c in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;
    }

    void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
    }

}
