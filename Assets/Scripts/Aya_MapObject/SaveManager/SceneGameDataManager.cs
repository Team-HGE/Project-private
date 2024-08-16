using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class SceneGameData
{
}
public class SceneGameDataManager : MonoBehaviour
{
    public SceneGameData GetData() // 세이브
    {
        SceneGameData sceneGameData = new SceneGameData();

        return sceneGameData;
    }
    public void ApplyGameData(SceneGameData sceneGameData) // 불러오기
    {
        
    }

    
}


