using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCGameData
{
    public NPC_Name myName;
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
            if (npcs[i].TryGetComponent<NPC>(out NPC nowNpc))
            {
                nPCGameData.myName = nowNpc.nPC_Name;
            }
            else
            {
                nPCGameData.myName = NPC_Name.Unknown;
            }
            npcData.Add(nPCGameData);
        }
        return npcData;
    }

    public void ApplyGameData(List<NPCGameData> npcGameData) // 불러오기
    {
        npcs = HotelFloorScene_DataManager.Instance.GetNPC_Transform();
        foreach(var nowNpc in npcs)
        {
            NPC_Name nowNPCName;
            if (nowNpc.TryGetComponent<NPC>(out NPC temp))
            {
                nowNPCName = temp.nPC_Name;
            }
            else
            {
                nowNPCName = NPC_Name.Unknown;
            }
            foreach (var nowData in npcGameData)
            {
                if (nowNPCName == nowData.myName)
                {
                    nowNpc.position = nowData.position.GetVector();
                    nowNpc.rotation = Quaternion.Euler(nowData.rotation.GetVector());
                    if (nowNPCName == NPC_Name.Unknown)
                    {
                        npcGameData.Remove(nowData);
                    }
                    break;
                }
            }
        }
        
    }
}
