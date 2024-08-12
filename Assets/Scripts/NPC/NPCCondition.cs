using UnityEngine;

public class NpcCondition : NpcData
{
    // ��Ʈ���� ����
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

    // NPC ���� ����
    // ��ȭ��, ��ȭ��, ����, ���

    public string ChangeNpcState(int ID, NpcState stateType)
    {
        switch (stateType)
        {
            case NpcState.Idle:
                NpcList[ID].state = NpcState.Idle;
                return "�����";
            case NpcState.Speaking:
                NpcList[ID].state = NpcState.Speaking;
                return "��ȭ��";
            case NpcState.Calling:
                NpcList[ID].state = NpcState.Calling;
                return "������";
            default:
                NpcList[ID].state = NpcState.Idle;
                return "�����";
        }
    }
}