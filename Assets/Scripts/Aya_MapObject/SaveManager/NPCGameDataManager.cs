using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class NPCGameData
{
    
}
public class NPCGameDataManager : MonoBehaviour
{
    public NPCGameData GetData() // ���̺�
    {
        NPCGameData npcData = new NPCGameData();

        return npcData;
    }

    public void ApplyGameData(NPCGameData npcGameData) // �ҷ�����
    {

    }
}
