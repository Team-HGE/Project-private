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

        if (scriptSO == null) { Debug.Log("지금은 내보낼 스크립트가 없습니다. DialogueManager > Script 컴포넌트에 script SO 파일을 드래그앤드롭 해주세요"); return;};

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
                //Debug.Log("잠깐 정지하고 선택지 출력합니다.");
  
                UtilSB.AppendText(uiDialogue.bodyText, sbBody, scriptSO.bodyTexts[i - 1]);

                DialogueManager.Instance.answer.StartAnswer();
                yield return new WaitUntil(() => DialogueManager.Instance.answer.answerSO.nowAnswer != 0);

                DialogueManager.Instance.answer.answerSO.nowAnswer = 0;

                continue;
            }

            curPrintLine = TextEffect.Typing(uiDialogue.bodyText, sbBody, scriptSO.bodyTexts[i]);
            yield return StartCoroutine(curPrintLine);

            //Debug.Log("E 키로 진행하세요");
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