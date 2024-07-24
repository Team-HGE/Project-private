using System;
using UnityEngine;

public enum NpcState
{
    Idle,
    Speaking,
    Calling,
    Mutated,
    Dead,
    Item
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
    public string objName;
    public NpcState state;
    public Emotion emotion;
    public Sprite[] illusts;
    [TextArea] public string[] testDialogue;
}