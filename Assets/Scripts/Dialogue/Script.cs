using System.Collections;
using UnityEngine;

public class Script: Dialogue
{
    public ScriptSO scriptSO;

    private void InitScript(ScriptSO _script)
    {
        scriptSO = _script;
    }

    public void StartScript()
    {
        if (nowTalking) return;
        OpenDialogue();
        InitScript(scriptSO);
        StartCoroutine(PrintScript());
    }

    public IEnumerator PrintScript()
    {
        titleText.text = sbTitle.Clear().ToString();
        bodyText.text = sbBody.Clear().ToString();

        portrait.sprite = null;

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            UtilSB.SetText(titleText, sbTitle, scriptSO.speakers[i]);

            SetImage(portrait, scriptSO.images[i]);

            if (portrait.sprite == null) portrait.transform.localScale = Vector3.zero;
            else
            {
                portrait.transform.localScale = Vector3.one;
            }

            curPrintLine = TextEffect.Typing(bodyText, sbBody, scriptSO.bodyTexts[i]);
            yield return StartCoroutine(curPrintLine);

            //Debug.Log("E 키로 진행하세요");
            //yield return new WaitUntil(() => Input.GetKey(KeyCode.E));

            yield return new WaitForSeconds(1f);

            ClearDialogue();
        }

        CloseDialogue();

        nowTalking = false;

        yield return null;
    }
}