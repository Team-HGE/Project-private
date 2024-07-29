using TMPro;
using UnityEngine;

public class Answer : DialogueSetting
{
    public AnswerSO answerSO;
    //private TextMeshProUGUI[] answerTexts;

    public void Init()
    {
        ui = GetComponent<UIDialogue>();
        ui.AnswerCanvas.SetActive(false);
    }

    public void InitAnswer(AnswerSO _answer)
    {
        answerSO = _answer;
        answerSO.nowAnswer = 0;
        Debug.Log("������ �ʱ�ȭ �Ϸ�");
    }

    public void Print()
    {
        Debug.Log("������ ����");
        InitAnswer(answerSO);

        //for(int i = 0; i < answerSO.answers.Length; i++)
        {
            // ������ ������ŭ answerTextBtn Instantiate �ϱ�
            // answerText ui �����ͼ� answerSO.answers[i] �Է��ϱ�
            // �����ϸ� ������ ��������
        }

        ui.answerText1.text = answerSO.answers[0];
        ui.answerText2.text = answerSO.answers[1];
        ui.AnswerCanvas.SetActive(true);

        // Ŀ���� OFF
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // TODO: ������ ��� ���
    }

    public void PickAnswer()
    {
        Debug.Log("1�� ������ Ŭ����");
        // �÷��̾� ī���� ���� ����
        ui.AnswerCanvas.SetActive(false);
        answerSO.nowAnswer = 1;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void PickAnswer2()
    {
        Debug.Log("2�� ������ Ŭ����");
        // �÷��̾� ī���� ���� ����
        ui.AnswerCanvas.SetActive(false);
        answerSO.nowAnswer = 2;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}