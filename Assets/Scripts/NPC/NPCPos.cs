using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCPos : MonoBehaviour
{
    static public NPCPos Instance { get; private set; }

    [Header("NPC")]
    public List<GameObject> NPCList;

    [Header("Time Position")]
    public GameObject dayTimePos;
    public GameObject nightPos;

    private void Awake()
    {
        Instance = this;

        SetDayTimePos();
    }


    public void SetDayTimePos()
    {        
        // 낮 npc 대기 위치
        Transform parentTransform = dayTimePos.transform;
        int totalChildren = parentTransform.childCount;

        Debug.Log($"자식수 : {totalChildren}");
        Debug.Log($"0 : {parentTransform.GetChild(0).transform.position}");

        if (NPCList.Count <= 0)
        {
            Debug.LogError("npc 가 없습니다.");
            return;
        }


        if (totalChildren < NPCList.Count)
        {
            Debug.LogError("npc 를 배치할 공간이 부족합니다");
            return;
        }

        List<int> tempIndex = new List<int>();
        for (int i = 0; i < totalChildren; i++)
        {
            tempIndex.Add(i);
        }
                
        List<int> ranIndex = Shuffle(tempIndex);

        for (int i = 0; i < NPCList.Count; i++)
        {
            NPCList[i].transform.position = parentTransform.GetChild(ranIndex[i]).transform.position;
        }

    }

    public void SetNightNPCpos()
    {
        Transform parentTransform = nightPos.transform;
        int totalChildren = parentTransform.childCount;

        for (int i = 0; i < NPCList.Count; i++)
        {
            NPCList[i].transform.position = parentTransform.GetChild(i).transform.position;
        }
    }



    public List<int> Shuffle(List<int> list)
    {
        System.Random ran = new System.Random();

        List<int> getIndex = new List<int>();
        int n = NPCList.Count;

        while (n > 0)
        {
            n--;
            int ranNum = ran.Next(list.Count);
            getIndex.Add(list[ranNum]);
            list.RemoveAt(ranNum);
        }

        return getIndex;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
