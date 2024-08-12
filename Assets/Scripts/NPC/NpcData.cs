using System.Collections.Generic;
using UnityEngine;

public class NpcData: MonoBehaviour
{
    public NpcCondition condition;

    public List<NPC_SO> NpcList = new List<NPC_SO>();
    public int storyIdx;

    // 게임 시작시 초기화
    public void Init()
    {
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
        return NpcList[ID].conversations[storyIdx];
    }

}