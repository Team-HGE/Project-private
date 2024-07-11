using UnityEngine;

public enum NPCStateType
{
    Speaking,
    Calling,
    Dead
}

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/NPC", order = 3)]
public class NPC_SO : ScriptableObject
{
    public string name;
    public NPCStateType stateTypes;
    public Sprite[] emotions;
    [TextArea] public string[] testDialogue;
}