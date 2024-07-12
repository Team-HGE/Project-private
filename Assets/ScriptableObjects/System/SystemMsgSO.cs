using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableObject", menuName = "ScriptableObject/SystemMsg", order = 2)]
public class SystemMsgSO : ScriptableObject
{
    // TODO: UI�� ��Ÿ�� �ý��� �޽���
    public string[] messages;

    // TODO: UI�� ��Ÿ�� ����
    [TextArea] public string[] tips;

    public AudioClip[] soundEffects;
}