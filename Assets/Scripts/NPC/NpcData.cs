using System.Collections.Generic;
using UnityEngine;

public class NpcData: MonoBehaviour
{
    public int storyIdx;

    public List<NPC_SO> NpcList = new List<NPC_SO>();

    // 게임 시작시 초기화
    public void Init()
    {
        storyIdx = 0;

        for (int i = 0; i < NpcList.Count; i++)
        {
            NpcList[i].state = 0;
            NpcList[i].emotion = 0;
            NpcList[i].hadInteract = false;
            NpcList[i].stress = 0;
        }
    }

    // 스토리 시작시 초기화
    public void InitInteraction()
    {
        for (int i = 0; i < NpcList.Count; i++)
        {
            NpcList[i].emotion = 0;
            NpcList[i].hadInteract = false;
        }
    }

    // 해당 storyIdx의 상호작용 script를 불러옴
    public ScriptSO LoadNpcSO(int ID)
    {
        return NpcList[ID].conversations[storyIdx]; // IndexOutOfRangeException
    }

    // 스트레스 관리
    public void AllStressUp(float amount)
    {
        for (int i = 0; i < NpcList.Count; i++)
        {
            NpcList[i].stress = Mathf.Min(NpcList[i].stress + amount, 100.0f);
        }
    }

    public void StressUp(int ID, float amount)
    {
        NpcList[ID].stress = Mathf.Min(NpcList[ID].stress + amount, 100.0f);
    }

    public void StressDown(int ID, float amount)
    {
        NpcList[ID].stress = Mathf.Max(NpcList[ID].stress - amount, 0.0f);
    }

    // NPC 상태 제어
    // 대화중, 통화중, 변이, 사망

    public string ChangeNpcState(int ID, NpcState stateType)
    {
        switch (stateType)
        {
            case NpcState.Idle:
                NpcList[ID].state = NpcState.Idle;
                return "대기중";
            case NpcState.Speaking:
                NpcList[ID].state = NpcState.Speaking;
                return "대화중";
            case NpcState.Calling:
                NpcList[ID].state = NpcState.Calling;
                return "무전중";
            default:
                NpcList[ID].state = NpcState.Idle;
                return "대기중";
        }
    }

}