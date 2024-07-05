using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Dialogue", order = 0)]
public class DialogueSO : ScriptableObject
{
    public Sprite[] images;
    public string[] speakers;
    [TextArea] public string[] bodyTexts;
    public AudioClip[] audioClip;
}

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/Quest", order = 1)]
public class QuestSO : ScriptableObject
{
    // TODO: UI에 나타날 퀘스트 정보
    public string[] quests;
}

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/SystemMsg", order = 2)]
public class SystemMsgSO : ScriptableObject
{
    // TODO: UI에 나타날 시스템 메시지
    public string[] messages;

    // TODO: UI에 나타날 팁들
    [TextArea] public string[] tips;

    public AudioClip[] soundEffects;
}


