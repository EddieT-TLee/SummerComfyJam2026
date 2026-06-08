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
    public GameObject dialoguoPanel;
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
        if(CanInteract())
            Debug.Log("bruh moment");
    }
    
}
