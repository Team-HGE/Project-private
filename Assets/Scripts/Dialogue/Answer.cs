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
        //Debug.Log("������ �ʱ�ȭ �Ϸ�");
    }

    public void StartAnswer()
    {
        //Debug.Log("������ ����");
        InitAnswerData(answerSO);

        uiDialogue.answerText1.text = answerSO.answers[0];
        uiDialogue.answerText2.text = answerSO.answers[1];

        uiDialogue.AnswerCanvas.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public void UseAnswer()
    {
    }

    public void PickAnswer()
    {
        //Debug.Log("1�� ������ Ŭ����");
        uiDialogue.AnswerCanvas.SetActive(false);
        answerSO.nowAnswer = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void PickAnswer2()
    {
        //Debug.Log("2�� ������ Ŭ����");
        uiDialogue.AnswerCanvas.SetActive(false);
        answerSO.nowAnswer = 2;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}