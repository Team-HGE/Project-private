using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/SystemMsg", order = 2)]
public class SystemMsgSO : ScriptableObject
{
    // TODO: UI에 나타날 시스템 메시지
    public string[] messages;

    // TODO: UI에 나타날 팁들
    [TextArea] public string[] tips;

    public AudioClip[] soundEffects;
}