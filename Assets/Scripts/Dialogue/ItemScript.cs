using UnityEngine;
using System.Collections;
using UnityEngine.ProBuilder.MeshOperations;

public class ItemScript : DialogueSetting, IScript
{
    [HideInInspector]
    private ScriptSO scriptSO;
    private Item item;

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

        // 상호작용 중인 오브젝트 판별
        GameObject nowInteracting = GameManager.Instance.player.curInteractableGameObject;
        item = nowInteracting.GetComponent<Item>();

        // item 이 아닐 경우
        if (item == null) { Debug.Log("Item이 아닙니다. 또는 Item 컴포넌트가 없습니다."); }

        // 플레이어 정지 - 카메라 에러***
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();

        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[i]);
            ui.SetImage(ui.portrait, scriptSO.images[i]);
            ui.CheckNullTitle(scriptSO.speakers[i]);

            if (scriptSO.bodyTexts[i] == "PickAnswer")
            {
                //Debug.Log("잠깐 정지하고 선택지 출력합니다.");

                UtilSB.AppendText(ui.bodyText, sbBody, scriptSO.bodyTexts[i - 1]);

                DialogueManager.Instance.answer.Print();
                yield return new WaitUntil(() => DialogueManager.Instance.answer.answerSO.nowAnswer != 0);

                DialogueManager.Instance.answer.answerSO.nowAnswer = 0;
                continue;
            }

            curPrintLine = TextEffect.Typing(ui.bodyText, sbBody, scriptSO.bodyTexts[i]);
            yield return StartCoroutine(curPrintLine);

            //Debug.Log("좌클릭으로 진행하세요");
            yield return waitLeftClick;
            yield return waitTime;

            ui.ClearDialogue(sbTitle, sbBody);
        }

        ui.CloseDialogue();
        isTalking = false;

        Debug.Log("아이템 스크립트 종료");

        // 플레이어 행동 가능 - 카메라 에러***
        GameManager.Instance.PlayerStateMachine.Player.PlayerControllOnOff();

        yield return null;
    }
}