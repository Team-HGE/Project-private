using System;
using UnityEngine;

public enum NPCStateType
{
    Idle,
    Speaking,
    Calling,
    Mutated,
    Dead
}

public enum NPCEmotion
{
    Default,
    Embarrassed,
    Rage,
    Worried
}

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/NPC", order = 3)]
public class NPC_SO : ScriptableObject
{
    public string npcName;
    public NPCStateType stateType;
    public Sprite[] emotions;
    [TextArea] public string[] testDialogue;
}