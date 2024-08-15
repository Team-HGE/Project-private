using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class QuestGameData
{

}
public class QuestGameDataManager : MonoBehaviour
{
    public QuestGameData GetData() // 세이브
    {
        QuestGameData questGameData = new QuestGameData();

        return questGameData;
    }

    public void ApplyGameData(QuestGameData questGameData) // 불러오기
    {

    }
}
