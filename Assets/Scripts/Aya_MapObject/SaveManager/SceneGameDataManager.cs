using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class SceneGameData
{
    public string objectName; // 오브젝트의 이름이나 ID
    public Vector3 position;  // 오브젝트의 위치
    public Quaternion rotation; // 오브젝트의 회전
    public bool isActive; // 오브젝트의 활성화 상태
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


