﻿using UnityEngine;
using System.Collections;
using System.Text;
using System;

public class NPCScript : DialogueSetting, IScript
{
    private NPC_SO npcSO;
    private ScriptSO scriptSO;
    public event Action playEvent;

    public void InitNPC(NpcData data, int ID)
    {
        npcSO = data.NpcList[ID];
        Init(data.LoadNpcSO(ID));

        // NPC 상태 대화중으로 변경, 스트레스 지수 감소
        data.ChangeNpcState(ID, NpcState.Speaking);
        data.StressDown(ID, 10);
    }

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


        // 상호작용 중인 오브젝트 판별
        //GameObject nowInteracting = GameManager.Instance.player.curInteractableGameObject;
        //npc = nowInteracting.GetComponent<NPC>();

        // npc 가 아닐 경우
        //if (npc == null) { Debug.Log("NPC가 아닙니다. 또는 NPC 컴포넌트가 없습니다."); return; }

        // NPC 감정 상태 초기화
        //npc.ChangeNpcState(NpcState.Idle);

        ui.OpenDialogue();

        // 코루틴이 실행 전 대화했음으로 변경됨
        npcSO.hadInteract = true;

        StartCoroutine(PrintScript());
    }

    private IEnumerator PrintScript()
    {
        ui.ClearDialogue(sbTitle, sbBody);

        for (int i = 0; i < scriptSO.bodyTexts.Length; i++)
        {
            if (!isTalking) { Debug.Log("실행중인 코루틴을 종료합니다."); StopAllCoroutines(); InitDialogueSetting(); break; }

            // 말하는 NPC 이름 - 대화중
            if (scriptSO.speakers[i] != "")
                UtilSB.SetText(ui.titleText, sbTitle, scriptSO.speakers[i] + " - " + PrintNpcState(npcSO.state));

            ui.SetPortrait(ui.portrait, scriptSO.portraits[i]);
            ui.CheckNullIndex(scriptSO.speakers[i]);

            curPrintLine = TextEffect.Typing(ui.bodyText, sbBody, scriptSO.bodyTexts[i]);
            yield return StartCoroutine(curPrintLine);

            yield return waitLeftClick;
            yield return waitTime;

            ui.ClearDialogue(sbTitle, sbBody);
        }

        ui.CloseDialogue();
        isTalking = false;

        //해당 NPC 대화 기회 소모
        //NPC 상태 대기중으로 변경

        //npcSO.hadInteract = true;

        Debug.Log($"{npcSO.name} 대화했음");

        playEvent?.Invoke();
        playEvent = null;
        npcSO.state = NpcState.Idle;

        yield return null;
    }

    public string PrintNpcState(NpcState stateType)
    {
        switch (stateType)
        {
            case NpcState.Idle:
                return "대기중";
            case NpcState.Speaking:
                return "대화중";
            case NpcState.Calling:
                return "무전중";
            case NpcState.Mutated:
                return "변이중";
            default:
                return "대기중";
        }
    }
}