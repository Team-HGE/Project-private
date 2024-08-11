using System;
using UnityEngine;

public enum NpcState
{
    Idle,
    Speaking,
    Calling,
    Mutated,
    Dead,
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
    public NpcState state;
    public Emotion emotion;
    public Sprite[] standings;
    public bool isInteracted = false;
}