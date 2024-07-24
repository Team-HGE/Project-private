using System.Collections;
using UnityEngine;

public class Script: Dialogue
{
    public ScriptSO scriptSO;

    private void InitScript(ScriptSO _script)
    {
        uiDialogue = GetComponent<UIDialogue>();
        scriptSO = _script;

    }

    public void StartScript()
    {
        if (nowTalking) return;
        InitScript(scriptSO);

        if (scriptSO == null) { Debug.Log("������ ������ ��ũ��Ʈ�� �����ϴ�. DialogueManager > Script ������Ʈ�� script SO ������ �巡�׾ص�� ���ּ���"); return;};

        uiDialogue.OpenDS();

        uiDialogue.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    public IEnumerator PrintScript()
    {
        uiDialogue.titleText.text = sbTitle.Clear().ToString();
        uiDialogue.bodyText.text = sbBody.Clear().ToString();

        uiDialogue.portrait.sprite = null;

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            UtilSB.SetText(uiDialogue.titleText, sbTitle, scriptSO.speakers[i]);

            uiDialogue.SetImage(uiDialogue.portrait, scriptSO.images[i]);

            uiDialogue.CheckSpeakerNull(scriptSO.speakers[i]);

            if (scriptSO.bodyTexts[i] == "PickAnswer")
            {
                //Debug.Log("��� �����ϰ� ������ ����մϴ�.");
  
                UtilSB.AppendText(uiDialogue.bodyText, sbBody, scriptSO.bodyTexts[i - 1]);

                DialogueManager.Instance.answer.StartAnswer();
                yield return new WaitUntil(() => DialogueManager.Instance.answer.answerSO.nowAnswer != 0);

                DialogueManager.Instance.answer.answerSO.nowAnswer = 0;

                continue;
            }

            curPrintLine = TextEffect.Typing(uiDialogue.bodyText, sbBody, scriptSO.bodyTexts[i]);
            yield return StartCoroutine(curPrintLine);

            //Debug.Log("E Ű�� �����ϼ���");
            //yield return new WaitUntil(() => Input.GetKey(KeyCode.E));

            yield return new WaitForSeconds(3f);

            ClearDialogue();
        }

        uiDialogue.CloseDialogue();

        nowTalking = false;
        uiDialogue.CloseDS();


        yield return null;
    }
}