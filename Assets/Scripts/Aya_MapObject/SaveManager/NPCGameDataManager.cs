using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCGameData
{
    public NPCGameData()
    {
        position = new SerializableVecter3();
        rotation = new SerializableVecter3();
    }
    public SerializableVecter3 position; // 플레이어 위치 정보
    public SerializableVecter3 rotation; // 플레이어 회전 정보
}
public class NPCGameDataManager : MonoBehaviour
{
    public Transform[] npcs;
    public List <NPCGameData> GetData() // 세이브
    {
        List<NPCGameData> npcData = new List<NPCGameData>();
        npcs = HotelFloorScene_DataManager.Instance.GetNPC_Transform();
        for (int i = 0; i < npcs.Length; i++)
        {
            NPCGameData nPCGameData = new NPCGameData();
            nPCGameData.position.SetVector(npcs[i].position);
            nPCGameData.rotation.SetVector(npcs[i].rotation.eulerAngles);

            npcData.Add(nPCGameData);
        }
        return npcData;
    }

    public void ApplyGameData(List<NPCGameData> npcGameData) // 불러오기
    {
        npcs = HotelFloorScene_DataManager.Instance.GetNPC_Transform();
        for (int i = 0; i < npcGameData.Count; i++)
        {
            if (npcs.Length < i)
            {
                return;
            }
            npcs[i].position = npcGameData[i].position.GetVector();
            npcs[i].rotation = Quaternion.Euler(npcGameData[i].rotation.GetVector());
        }
    }
}
