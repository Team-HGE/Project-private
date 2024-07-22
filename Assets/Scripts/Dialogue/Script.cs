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

        if (scriptSO == null) { Debug.Log("지금은 내보낼 스크립트가 없습니다."); return;};
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

            curPrintLine = TextEffect.Typing(uiDialogue.bodyText, sbBody, scriptSO.bodyTexts[i]);
            yield return StartCoroutine(curPrintLine);

            //Debug.Log("E 키로 진행하세요");
            //yield return new WaitUntil(() => Input.GetKey(KeyCode.E));

            yield return new WaitForSeconds(3f);

            ClearDialogue();
        }

        uiDialogue.CloseDialogue();

        nowTalking = false;

        yield return null;
    }
}