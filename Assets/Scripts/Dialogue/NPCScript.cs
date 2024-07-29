using UnityEngine;
using System.Text;
using System.Collections;

public class NPCScript : DialogueSetting, IScript
{
    public ScriptSO scriptSO;

    private NPC npc;
    private NPC_SO npcSO;

    public void Init(ScriptSO _script)
    {
        scriptSO = _script;
        ui.CloseDialogue();
    }

    public void Print()
    {
        // 스크립트 출력 중이면 새 대화를 시작하지 않음 
        if (isTalking) return;
        isTalking = true;

        // 상호작용 중인 오브젝트 판별
        GameObject nowInteracting = GameManager.Instance.player.curInteractableGameObject;
        npc = nowInteracting.GetComponent<NPC>();

        // npc가 아닐 경우
        if (npc == null)
        {
            Debug.Log("NPC가 아닙니다. 또는 NPC 정보가 없습니다.");
            return;
        }
        else
            npcSO = npc.npcSO;

        ui.OpenDialogue();
        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            // 말하는 NPC 이름 - 대화중 
            UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[i] + " - " + npc.ChangeNpcState(NpcState.Speaking));
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

        // NPC 감정 상태 해제
        npc.ChangeNpcState(NpcState.Idle);

        yield return null;
    }
}