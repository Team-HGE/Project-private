using System.Collections;
using UnityEngine;

public class KarmaScript : DialogueSetting, IScript
{
    [HideInInspector]
    private ScriptSO scriptSO;

    public void Init(ScriptSO _script)
    {
        scriptSO = _script;
        InitUI();
        ui.CloseDialogue();
    }

    public void Print()
    {
        // 스크립트 출력 중이면 새 대화를 시작하지 않음 
        if (isTalking) return;
        isTalking = true;

        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < 1; i++)
        {
            int j = RandomKarmaIndex();
            UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[j]);
            ui.SetImage(ui.portrait, scriptSO.images[j]);
            ui.CheckNullTitle(scriptSO.speakers[j]);

            curPrintLine = TextEffect.Typing(ui.bodyText, sbBody, scriptSO.bodyTexts[j]);
            yield return StartCoroutine(curPrintLine);

            //Debug.Log("좌클릭으로 진행하세요");
            yield return waitLeftClick;
            yield return waitTime;

            ui.ClearDialogue(sbTitle, sbBody);
        }

        ui.CloseDialogue();
        isTalking = false;

        //Debug.Log("카르마 출력 완료");

        yield return null;
    }

    private int RandomKarmaIndex()
    {
        System.Random rng = new System.Random();
        int ranIndex = -1;

        //현재 카르마 수치 확인
        //카르마가 음수면 선, 양수면 악, 0이면 중립 메시지 출력

        if (GameManager.Instance.PlayerStateMachine.Player.Karma < 0)
        {
            // 카르마 선
            ranIndex = rng.Next(0, 5);
        }
        else if (GameManager.Instance.PlayerStateMachine.Player.Karma > 0)
        {
            // 카르마 악
            ranIndex = rng.Next(2, 7);
        }
        else
        {
            // 카르마 중립
            ranIndex = rng.Next(2, 5);
        }

        return ranIndex;
    }
}