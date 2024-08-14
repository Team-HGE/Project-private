using System.Collections;
using UnityEngine;

public class Monologue : DialogueSetting, IScript
{
    private ScriptSO scriptSO; // ���� private�� ��������
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
        // ��ũ��Ʈ ��� ���̸� �� ��ȭ�� �������� ����, ��ȭ ���� �ʱ�ȭ
        if (isTalking) { Debug.Log("������ ��ȭ�� �� �����ϴ�."); InitDialogueSetting(); return; }
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
        // ��ũ��Ʈ ��� ���̸� �� ��ȭ�� �������� ����, ��ȭ ���� �ʱ�ȭ
        if (isTalking) { Debug.Log("������ ��ȭ�� �� �����ϴ�."); InitDialogueSetting(); return; }
        isTalking = true;

        //Init(scriptSO);
        if (scriptSO == null) { Debug.Log("������ ������ ��ũ��Ʈ�� �����ϴ�. scriptSO null"); return; };

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

            //Debug.Log("��Ŭ������ �����ϼ���");
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

        //Debug.Log("��Ŭ������ �����ϼ���");
        yield return waitLeftClick;
        yield return waitTime;

        ui.ClearDialogue(sbTitle, sbBody);
        ui.CloseDialogue();
        isTalking = false;

        yield return null;
    }
}