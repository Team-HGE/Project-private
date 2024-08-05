using System.Collections;
using System.Text;
using UnityEngine;

public class StoryScript: DialogueSetting, IScript
{
    public ScriptSO scriptSO;

    public void Init(ScriptSO _script)
    {
        //sbTitle = new StringBuilder();
        //sbBody = new StringBuilder();
        scriptSO = null;
        scriptSO = _script;
        InitUI();
        ui.CloseDialogue();
    }

    public void Print()
    {
        // ��ũ��Ʈ ��� ���̸� �� ��ȭ�� �������� ����, ��ȭ ���� �ʱ�ȭ
        if (isTalking) { Debug.Log("������ ��ȭ�� �� �����ϴ�."); InitDialogueSetting(); return; }
        isTalking = true;

        Init(scriptSO);
        if (scriptSO == null) { Debug.Log("������ ������ ��ũ��Ʈ�� �����ϴ�. scriptSO null"); return; };

        StopAllCoroutines();
        ui.OpenBG();
        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        //if (!isTalking) { Debug.Log("�������� �ڷ�ƾ�� �����մϴ�."); StopAllCoroutines(); InitDialogueSetting(); }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            if (!isTalking) { Debug.Log("�������� �ڷ�ƾ�� �����մϴ�."); StopAllCoroutines(); InitDialogueSetting(); break; }

            UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[i]);

            ui.SetImage(ui.portrait, scriptSO.images[i]);
            ui.CheckNullTitle(scriptSO.speakers[i]);

            if (scriptSO.bodyTexts[i] == "PickAnswer")
            {
                //Debug.Log("��� �����ϰ� ������ ����մϴ�.");

                UtilSB.AppendText(ui.bodyText, sbBody, scriptSO.bodyTexts[i - 1]);

                DialogueManager.Instance.answer.Print();
                yield return new WaitUntil(() => DialogueManager.Instance.answer.answerSO.nowAnswer != 0);

                DialogueManager.Instance.answer.answerSO.nowAnswer = 0;
                continue;
            }

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
}