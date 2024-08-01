using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Answer : DialogueSetting
{
    public AnswerSO answerSO;

    public void Init()
    {
        InitUI();
        ui.AnswerCanvas.SetActive(false);
    }

    public void InitAnswer(AnswerSO _answer)
    {
        // ������ ����
        ui.answerText1.text = "";
        ui.answerText2.text = "";

        answerSO = _answer;
        answerSO.nowAnswer = 0;
        Debug.Log("������ �ʱ�ȭ �Ϸ�");
    }

    public void Print()
    {
        Debug.Log("���� ������ ����");
        InitAnswer(answerSO);

        string[] answersTemp = new string[answerSO.answers.Length];

        for(int i = 0; i < answerSO.answers.Length; i++)
        {
            answersTemp[i] = answerSO.answers[i];
        }

        ShuffleArray(answersTemp);

        ui.answerText1.text = answersTemp[0];
        ui.answerText2.text = answersTemp[1];
 
        //ui.answerText1.text = answerSO.answers[0];
        //ui.answerText2.text = answerSO.answers[1];
        ui.AnswerCanvas.SetActive(true);

        // Ŀ���� OFF
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //ui.AnswerCanvas.SetActive(false);

        // TODO: ������ ��� ���
    }

    public void ApplyAnswer()
    {
        GameObject clickObj = EventSystem.current.currentSelectedGameObject;
        //Debug.Log(clickObj);
        TextMeshProUGUI btnText = clickObj.GetComponentInChildren<TextMeshProUGUI>();

        if(btnText.text == answerSO.answers[0])
        {
            // �÷��̾� ī���� ���� ����
            //Debug.Log(btnText.text);
            GameManager.Instance.PlayerStateMachine.Player.Karma -= answerSO.karmaUpDown;
            answerSO.nowAnswer = 1;
        }
        else if (btnText.text == answerSO.answers[1])
        {
            // �÷��̾� ī���� ���� ����
            //Debug.Log(btnText.text);
            GameManager.Instance.PlayerStateMachine.Player.Karma += answerSO.karmaUpDown;
            answerSO.nowAnswer = 2;
        }
        else
        {
            //ī���� ��ȭ ����
            //Debug.Log(btnText.text);
            answerSO.nowAnswer = 3;
        }

        Debug.Log("���� ī���� ��ġ: " + GameManager.Instance.PlayerStateMachine.Player.Karma);
        ui.AnswerCanvas.SetActive(false);
    }

    //public void PickAnswer()
    //{
    //    Debug.Log("1�� ������ Ŭ����");
    //    // �÷��̾� ī���� ���� ����
    //    GameManager.Instance.PlayerStateMachine.Player.Karma += answerSO.karmaUpDown;
    //    ui.AnswerCanvas.SetActive(false);
    //    answerSO.nowAnswer = 1;
    //}
    //public void PickAnswer2()
    //{
    //    Debug.Log("2�� ������ Ŭ����");
    //    // �÷��̾� ī���� ���� ����
    //    GameManager.Instance.PlayerStateMachine.Player.Karma -= answerSO.karmaUpDown;
    //    ui.AnswerCanvas.SetActive(false);
    //    answerSO.nowAnswer = 2;
    //}

    private void ShuffleArray(string[] array)
    {
        System.Random rng = new System.Random();
        int n = array.Length;

        for (int i = n - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            Swap(array, i, j);
        }
    }
    private void Swap(string[] array, int i, int j)
    {
        string temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }
}