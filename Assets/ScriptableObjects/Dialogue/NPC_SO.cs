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
    [Header("Info")]
    public string npcName;
    public NpcState state;
    public Emotion emotion;
    //public Sprite[] standings;

    [Header("Interact")]
    public float stress = 0; // 0에서 100까지
    public bool hadInteract = false;

    [Header("Script Per Day")]
    public ScriptSO[] conversations;
}