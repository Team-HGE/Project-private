using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Answer", order = 4)]

public class AnswerSO: ScriptableObject
{
    public int[] answerIDs;
    public string[] answers;
}