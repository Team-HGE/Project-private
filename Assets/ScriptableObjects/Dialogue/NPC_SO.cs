using System;
using UnityEngine;

public enum NpcState
{
    Idle, //대기중
    Speaking, //대화중
    Calling, //무전중
    Mutated, //변이중
    Dead, //사망
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