using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Answer", order = 4)]

public class AnswerSO: ScriptableObject
{
    public int nowAnswer;
    public string[] answers;
    public float karmaUpDown;
    public bool isLocked;
    [TextArea] public string[] results;
}