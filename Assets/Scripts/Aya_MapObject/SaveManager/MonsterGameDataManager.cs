using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MonsterGameData
{

}

public class MonsterGameDataManager : MonoBehaviour
{
    public MonsterGameData GetData() // 세이브
    {
        MonsterGameData monsterGameData = new MonsterGameData();

        return monsterGameData;
    }

    public void ApplyGameData(MonsterGameData monsterGameData) // 불러오기
    {

    }
}
