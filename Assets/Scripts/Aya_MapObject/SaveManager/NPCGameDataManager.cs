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
    public NPCGameData GetData() // 세이브
    {
        NPCGameData npcData = new NPCGameData();

        return npcData;
    }

    public void ApplyGameData(NPCGameData npcGameData) // 불러오기
    {

    }
}
