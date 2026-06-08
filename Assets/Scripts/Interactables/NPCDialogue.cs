using System;
using UnityEngine;


/// <summary>
/// I feel like this is very self-explanatory
/// If you really don't understand contact me
/// </summary>
[CreateAssetMenu(fileName = "NewNPCDialogue", menuName = "Scriptable Objects/NPCDialogue")]
public class NPCDialogue : ScriptableObject
{
    public String npcName;
    public Sprite npcPortrait;
    public string[] dialogueLines;
    public float typingSpeed = 0.0f;
    public float voicePitch = 1f;
    

}
