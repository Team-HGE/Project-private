using System.Text;
using TMPro;
using UnityEngine;

public class Answer : DialogueSetting
{
    public AnswerSO answerSO;

    public void Init()
    {
        InitUI();
        // ������ ����
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
        string[] answerTexts = new string[answerSO.answers.Length];

        for(int i = 0; i < answerSO.answers.Length; i++)
        {
            // ������ ������ŭ answerTextBtn Instantiate �ϱ�
            // answerText ui �����ͼ� answerSO.answers[i] �Է��ϱ�
            // �����ϸ� ������ ��������

            answerTexts[i] = answerSO.answers[i];
        }

        ui.answerText1.text = answerSO.answers[0];
        ui.answerText2.text = answerSO.answers[1];
        ui.AnswerCanvas.SetActive(true);

        // Ŀ���� OFF
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //ui.AnswerCanvas.SetActive(false);

        // TODO: ������ ��� ���
    }

    public void PickAnswer()
    {
        Debug.Log("1�� ������ Ŭ����");
        // �÷��̾� ī���� ���� ����
        GameManager.Instance.PlayerStateMachine.Player.Karma += answerSO.karmaUpDown;
        ui.AnswerCanvas.SetActive(false);
        answerSO.nowAnswer = 1;
    }
    public void PickAnswer2()
    {
        Debug.Log("2�� ������ Ŭ����");
        // �÷��̾� ī���� ���� ����
        GameManager.Instance.PlayerStateMachine.Player.Karma -= answerSO.karmaUpDown;
        ui.AnswerCanvas.SetActive(false);
        answerSO.nowAnswer = 2;
    }

    private void ShuffleArray(int[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;

        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            Swap(array, i, j);
        }
    }
    private void Swap(int[] array, int i, int j)
    {
        int temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }
}