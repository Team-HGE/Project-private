using UnityEngine;

public class Answer : Dialogue
{
    public AnswerSO answerSO;

    public void InitAnswer()
    {
        uiDialogue = GetComponent<UIDialogue>();
        uiDialogue.AnswerCanvas.SetActive(false);
    }

    public void InitAnswerData(AnswerSO _answer)
    {
        answerSO = _answer;
        answerSO.nowAnswer = 0;
        Debug.Log("선택지 초기화 완료");
    }

    public void StartAnswer()
    {
        Debug.Log("선택지 시작");
        InitAnswerData(answerSO);

        uiDialogue.answerText1.text = answerSO.answers[0];
        uiDialogue.answerText2.text = answerSO.answers[1];

        uiDialogue.AnswerCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        // 커서를 화면에 표시합니다.
        Cursor.visible = true;

    }

    public void UpdateAnswer()
    {
        Debug.Log("선택지 업데이트 완료");
    }

    public void PickAnswer()
    {
        Debug.Log("1번 선택지 클릭됨");
        uiDialogue.AnswerCanvas.SetActive(false);
        answerSO.nowAnswer = 1;
    }
    public void PickAnswer2()
    {
        Debug.Log("2번 선택지 클릭됨");
        uiDialogue.AnswerCanvas.SetActive(false);
        answerSO.nowAnswer = 2;
    }
}