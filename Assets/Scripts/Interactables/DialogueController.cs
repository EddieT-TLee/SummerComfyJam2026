using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueController : MonoBehaviour
{
    public static DialogueController instance { get; private set; } // Singleton
    
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public GameObject portraitPanel;
    public Image portrait;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;

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
        nameText.text = npcName;
        portrait.sprite = npcPortrait;
        portraitPanel.SetActive(true);
    }
    
    public void SetMingameInfo(string npcName)
    {
        nameText.text = npcName;
        portraitPanel.SetActive(false);
    }
    
    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
        {
            Destroy(child.gameObject);
        }
    }

    public void CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
    }
}
