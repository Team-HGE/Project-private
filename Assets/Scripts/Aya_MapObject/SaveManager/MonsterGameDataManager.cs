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
    public MonsterGameData GetData() // ���̺�
    {
        MonsterGameData monsterGameData = new MonsterGameData();

        return monsterGameData;
    }

    public void ApplyGameData(MonsterGameData monsterGameData) // �ҷ�����
    {

    }
}
