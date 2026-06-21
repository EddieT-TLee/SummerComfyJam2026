using System.Collections;
using UnityEngine;


public class NPC : MonoBehaviour, IInteractable
{
    public NPCDialogue dialogueData;
    public NPCDialogue questStartedDialogue;
    public NPCDialogue questCompletedDialogueData;
    public string questName;
    private bool questCompleted = false;

    private DialogueController dialogueUI;
    private int dialogueIndex;
    private DialogueLine[] dialogueLines;
    private bool isTyping, isDialogueActive, hasChoice;
    private bool reset;
    private Sprite currentPortrait;
    private Sprite[]  portraits;
    private Animator animator;
    private string currentTalkingAnim;
    private NPCDialogue pendingDialogueData;

    private void Start()
    {
        dialogueUI = DialogueController.instance; 
        
        try
        {
            if (QuestController.instance != null &&
                QuestController.instance.GetQuestStatus(questName) == QuestStatus.Completed)
            {
                questCompleted = true;
                ApplyDialogueData(questCompletedDialogueData);
            } else if (QuestController.instance != null &&
                       QuestController.instance.GetQuestStatus(questName) == QuestStatus.Active)
            {
                ApplyDialogueData(questStartedDialogue);
            }
        }
        catch (System.Exception)
        {
            // Quest doesn't exist or isn't completed, so keep and load default dialogueData
        }

        if (!questCompleted) ApplyDialogueData(dialogueData);
        animator = GetComponent<Animator>();
        currentTalkingAnim = "Idle";
    }

    private void Update()
    {
        try
        {
            
            if (QuestController.instance != null &&
                QuestController.instance.GetQuestStatus(questName) == QuestStatus.Completed && 
                !questCompleted)
            {
                questCompleted = true;
                ChangeNPCDialogue(questCompletedDialogueData);
            }
        }
        catch (System.Exception)
        {
        }
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
        PauseManager.IsPaused = isDialogueActive;
        dialogueIndex = 0; 
        currentPortrait = portraits[dialogueLines[dialogueIndex].portraitIndex];
        
        dialogueUI.SetNPCInfo(dialogueData.npcName, currentPortrait);
        dialogueUI.ShowDialogueUI(true);
        FacePlayer();
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
        if (choice.onChoiceSelected != null)
        {
            foreach (var action in choice.onChoiceSelected)
            {
                action.Invoke(this);
            }
        }

        currentPortrait = portraits[choice.portraitIndex];
        dialogueUI.SetNPCInfo(dialogueData.npcName, currentPortrait);
        dialogueUI.ClearChoices();
        // Convert the array of choice lines into dialogue lines
        dialogueLines = choice.chudLines;

        if (choice.resetDialogue)
        {
            reset = true;
        }
        dialogueIndex = 0;

        DisplayCurrentLine();
    }

    void FacePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        
        Vector2 direction =  player.transform.position - transform.position;

        // Based on the X and Y compoenent of the direction vector determines which direction the player is
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            currentTalkingAnim = direction.x > 0 ? "Talking_right" : "Talking_left";
        }
        else if (Mathf.Abs(direction.x) < Mathf.Abs(direction.y))
        {
            currentTalkingAnim = direction.y < 0 ? "Talking_front" : "Talking_back";
        }
        
        // Default to idle if Npc doesn't have talking animation 
        int stateHash = Animator.StringToHash(currentTalkingAnim);
    
        if (animator.HasState(0, stateHash))
        {
            animator.Play(currentTalkingAnim);
        }
        else
        {
            animator.Play("Idle");
        }
        
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
        PauseManager.IsPaused = isDialogueActive;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
        if (reset) // Reset to start of Dialogue data
        {
            dialogueLines = dialogueData.dialogueLines;
            reset = false;
        }

        if (pendingDialogueData != null)
        {
            ApplyDialogueData(pendingDialogueData);
            pendingDialogueData = null;
        }
        
        if (animator != null)
            animator.Play("Idle");
    }

    public void ChangeNPCDialogue(NPCDialogue npcDialogue)
    {
        if (isDialogueActive)
        {
            pendingDialogueData = npcDialogue;
            return;
        }

        ApplyDialogueData(npcDialogue);
    }

    private void ApplyDialogueData(NPCDialogue npcDialogue)
    {
        dialogueData = npcDialogue;
        dialogueLines = dialogueData.dialogueLines;
        portraits = dialogueData.npcPortraitSprites;
        dialogueIndex = 0;
        reset = false;
    }
}
