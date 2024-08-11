using UnityEngine;
using System.Collections;
using System.Text;

public class NPCScript : DialogueSetting, IScript
{
    [HideInInspector]
    private ScriptSO scriptSO;
    private NPC npc;

    public void Init(ScriptSO _script)
    {
        scriptSO = null;
        scriptSO = _script;
        InitUI();
        ui.CloseDialogue();
    }

    public void Print()
    {
        // 스크립트 출력 중이면 새 대화를 시작하지 않음, 대화 내용 초기화
        if (isTalking) { Debug.Log("지금은 대화할 수 없습니다."); InitDialogueSetting(); return; }
        isTalking = true;

        //플레이어 움직임 정지

        // 상호작용 중인 오브젝트 판별
        GameObject nowInteracting = GameManager.Instance.player.curInteractableGameObject;
        npc = nowInteracting.GetComponent<NPC>();

        // npc 가 아닐 경우
        if (npc == null) { Debug.Log("NPC가 아닙니다. 또는 NPC 컴포넌트가 없습니다."); return; }

        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        //if (!isTalking) { Debug.Log("실행중인 코루틴을 종료합니다."); StopAllCoroutines(); InitDialogueSetting();}

        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            if (!isTalking) { Debug.Log("실행중인 코루틴을 종료합니다."); StopAllCoroutines(); InitDialogueSetting(); break; }

            // 말하는 NPC 이름 - 대화중
            if (scriptSO.speakers[i] != "")
                UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[i] + " - " + npc.ChangeNpcState(NpcState.Speaking));

            ui.SetPortrait(ui.portrait, scriptSO.portraits[i]);
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
            //Debug.Log(sbTitle); null 잘됨
        }

        ui.CloseDialogue();
        isTalking = false;

        //플레이어 움직임 재개

        // NPC 감정 상태 해제
        npc.ChangeNpcState(NpcState.Idle);

        //해당 NPC 대화 기회 소모 
        npc.npcSO.isInteracted = true;

        yield return null;
    }
}