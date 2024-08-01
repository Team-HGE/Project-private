using System.Text;
using TMPro;
using UnityEngine;

public class Answer : DialogueSetting
{
    public AnswerSO answerSO;

    public void Init()
    {
        InitUI();
        // 가비지 제거
        ui.AnswerCanvas.SetActive(false);
    }

    public void InitAnswer(AnswerSO _answer)
    {
        answerSO = _answer;
        answerSO.nowAnswer = 0;
        Debug.Log("선택지 초기화 완료");
    }

    public void Print()
    {

        Debug.Log("선택지 시작");
        InitAnswer(answerSO);
        string[] answerTexts = new string[answerSO.answers.Length];

        for(int i = 0; i < answerSO.answers.Length; i++)
        {
            // 선택지 개수만큼 answerTextBtn Instantiate 하기
            // answerText ui 가져와서 answerSO.answers[i] 입력하기
            // 가능하면 순서는 랜덤으로

            answerTexts[i] = answerSO.answers[i];
        }

        ui.answerText1.text = answerSO.answers[0];
        ui.answerText2.text = answerSO.answers[1];
        ui.AnswerCanvas.SetActive(true);

        // 커서락 OFF
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //ui.AnswerCanvas.SetActive(false);

        // TODO: 선택지 결과 출력
    }

    public void PickAnswer()
    {
        Debug.Log("1번 선택지 클릭됨");
        // 플레이어 카르마 스탯 증감
        GameManager.Instance.PlayerStateMachine.Player.Karma += answerSO.karmaUpDown;
        ui.AnswerCanvas.SetActive(false);
        answerSO.nowAnswer = 1;
    }
    public void PickAnswer2()
    {
        Debug.Log("2번 선택지 클릭됨");
        // 플레이어 카르마 스탯 증감
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