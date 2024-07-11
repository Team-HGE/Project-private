using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Quest", order = 1)]
public class QuestSO : ScriptableObject
{
    // TODO: UI에 나타날 퀘스트 정보
    public string[] quests;
}