using System;
using UnityEngine;

public enum npcState
{
    Idle,
    Speaking,
    Calling,
    Mutated,
    Dead
}

public enum Emotion
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
    public npcState state;
    public Emotion emotion;
    public Sprite[] illusts;
    [TextArea] public string[] testDialogue;
}