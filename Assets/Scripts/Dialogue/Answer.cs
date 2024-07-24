using UnityEngine;

public class Answer : Dialogue
{
    public AnswerSO answer;

    public void InitAnswer()
    {
        Debug.Log("선택지 초기화 완료");
    }

    public void UpdateAnswer()
    {
        Debug.Log("선택지 업데이트 완료");
    }

    public void PickAnswer()
    {
        Debug.Log("1번 선택지 클릭됨");
    }
}