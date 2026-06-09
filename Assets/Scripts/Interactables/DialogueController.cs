using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance { get; private set; } // Singleton
    
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;


    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject); // Make sure only one instance
    }
    
    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void SetNPCInfo(string npcName, Sprite npcPortrait)
    {
        dialogueText.text = npcName;
        portraitImage.sprite = npcPortrait;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }
}
