using UnityEngine;

public class Answer : Dialogue
{
    public AnswerSO answer;

    public void InitAnswer()
    {
        Debug.Log("������ �ʱ�ȭ �Ϸ�");
    }

    public void UpdateAnswer()
    {
        Debug.Log("������ ������Ʈ �Ϸ�");
    }

    public void PickAnswer()
    {
        Debug.Log("1�� ������ Ŭ����");
    }
}