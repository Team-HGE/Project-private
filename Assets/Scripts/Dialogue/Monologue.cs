using System.Collections;
using UnityEngine;

public class Monologue : DialogueSetting, IScript
{
    private ScriptSO scriptSO; // 추후 private로 수정예정
    private string text;

    public void Init(ScriptSO _script)
    {
        scriptSO = null;
        scriptSO = _script;
        InitUI();
        ui.CloseDialogue();
    }

    public void Manual(string _text)
    {
        // 스크립트 출력 중이면 새 대화를 시작하지 않음, 대화 내용 초기화
        if (isTalking) { Debug.Log("지금은 대화할 수 없습니다."); InitDialogueSetting(); return; }
        isTalking = true;

        scriptSO = null;
        text = _text;
        InitUI();
        //ui.CloseDialogue();

        StopAllCoroutines();
        ui.OpenDialogue();
        StartCoroutine(PrintMonologue());
    }

    public void Print()
    {
        // 스크립트 출력 중이면 새 대화를 시작하지 않음, 대화 내용 초기화
        if (isTalking) { Debug.Log("지금은 대화할 수 없습니다."); InitDialogueSetting(); return; }
        isTalking = true;

        //Init(scriptSO);
        if (scriptSO == null) { Debug.Log("지금은 내보낼 스크립트가 없습니다. scriptSO null"); return; };

        StopAllCoroutines();
        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {

        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[i]);
            ui.CheckNullTitle(scriptSO.speakers[i]);

            curPrintLine = TextEffect.Typing(ui.bodyText, sbBody, scriptSO.bodyTexts[i]);
            yield return StartCoroutine(curPrintLine);

            //Debug.Log("좌클릭으로 진행하세요");
            yield return waitLeftClick;
            yield return waitTime;

            ui.ClearDialogue(sbTitle, sbBody);
        }

        ui.CloseDialogue();
        isTalking = false;

        yield return null;
    }

    private IEnumerator PrintMonologue()
    {
        ui.ClearDialogue(sbTitle, sbBody);

        UtilSB.SetText(ui.titleText, sbTitle, null);
        ui.CheckNullTitle(null);

        TextEffect.Typing(ui.bodyText, sbBody, text);
        //yield return StartCoroutine(curPrintLine);

        //Debug.Log("좌클릭으로 진행하세요");
        yield return waitLeftClick;
        yield return waitTime;

        ui.ClearDialogue(sbTitle, sbBody);
        ui.CloseDialogue();
        isTalking = false;

        yield return null;
    }
}