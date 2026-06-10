using System;
using UnityEngine;


/// <summary>
/// Everything should be self-explanatory
/// If you don't understand anything idk contact me
/// </summary>
[CreateAssetMenu(fileName = "NPCDialogue", menuName = "Scriptable Objects/NPCDialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcPortrait;
    public DialogueLine[] dialogueLines;
    public float typingSpeed = 0.05f;
}

[Serializable]
public struct DialogueLine
{
    [TextArea] public string text;
    public DialogueChoice[] choices; // if empty then there are no choices for line
}

[Serializable]
public struct DialogueChoice
{
    // If a choice needs to return back to the start (ex. player refused to take quest go back to start of dialogue tree)
    public bool resetDialogue; 
    public string choiceText;
    [TextArea] public string[] choiceLines;
    
}