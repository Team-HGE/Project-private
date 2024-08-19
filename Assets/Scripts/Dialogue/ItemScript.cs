using UnityEngine;
using System.Collections;

public class ItemScript : DialogueSetting, IScript
{
    [HideInInspector]
    private ScriptSO scriptSO;
    private Item item;
    public Quest quest;
    public SystemMsg systemMsg;

    public void Init(ScriptSO _script)
    {
        quest = DialogueManager.Instance.quest;
        systemMsg = DialogueManager.Instance.systemMsg;
        scriptSO = _script;
        InitUI();
        ui.CloseDialogue();
    }

    public void Print()
    {
        // 스크립트 출력 중이면 새 대화를 시작하지 않음 
        if (isTalking) return;
        isTalking = true;

        // 상호작용 중인 오브젝트 판별
        GameObject nowInteracting = GameManager.Instance.player.curInteractableGameObject;
        item = nowInteracting.GetComponent<Item>();

        // item 이 아닐 경우
        if (item == null) { Debug.Log("Item이 아닙니다. 또는 Item 컴포넌트가 없습니다."); }

        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[i]);
            //ui.SetPortrait(ui.portrait, scriptSO.portraits[i]);
            ui.CheckNullIndex(scriptSO.speakers[i]);

            //if (scriptSO.bodyTexts[i] == "PickAnswer")
            //{
            //    //Debug.Log("잠깐 정지하고 선택지 출력합니다.");

            //    UtilSB.AppendText(ui.bodyText, sbBody, scriptSO.bodyTexts[i - 1]);

            //    DialogueManager.Instance.answer.Print();
            //    yield return new WaitUntil(() => DialogueManager.Instance.answer.answerSO.nowAnswer != 0);

            //    DialogueManager.Instance.answer.answerSO.nowAnswer = 0;
            //    continue;
            //}

            switch (scriptSO.bodyTexts[i])
            {
                case var text when text.StartsWith("NewQuest"):
                    yield return HandleNewQuest(text);
                    continue;

                case var text when text.StartsWith("SystemMsg"):
                    yield return SystemMsg(text);
                    continue;

                case var text when text.StartsWith("NewTip"):
                    yield return Tips(text);
                    continue;

                default:
                    curPrintLine = TextEffect.Typing(ui.bodyText, sbBody, scriptSO.bodyTexts[i]);
                    break;
            }

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

    private IEnumerator HandleNewQuest(string questText)
    {
        if (questText.StartsWith("NewQuest"))
        {
            string questString = questText.Substring(8);
            if (int.TryParse(questString, out int questNumber))
            {
                quest.NextQuest(questNumber);
            }

            yield break;
        }
    }

    private IEnumerator Tips(string TipText)
    {
        if (TipText.StartsWith("NewTip"))
        {
            string TipString = TipText.Substring(6);
            if (int.TryParse(TipString, out int TipsNumber))
            {
                systemMsg.UpdateTipMessage(TipsNumber);
            }
        }
        yield break;
    }

    private IEnumerator SystemMsg(string SystemMsgText)
    {
        if (SystemMsgText.StartsWith("SystemMsg"))
        {
            string SystemMsgString = SystemMsgText.Substring(9);
            if (int.TryParse(SystemMsgString, out int SystemMsgNumber))
            {
                systemMsg.UpdateMessage(SystemMsgNumber);
            }
        }
        yield break;
    }

}