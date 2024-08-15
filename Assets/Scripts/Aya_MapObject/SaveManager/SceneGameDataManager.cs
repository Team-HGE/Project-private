using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[Serializable]
public class SceneGameData
{
    public string objectName; // ������Ʈ�� �̸��̳� ID
    public Vector3 position;  // ������Ʈ�� ��ġ
    public Quaternion rotation; // ������Ʈ�� ȸ��
    public bool isActive; // ������Ʈ�� Ȱ��ȭ ����
}
public class SceneGameDataManager : MonoBehaviour
{
    public SceneGameData GetData() // ���̺�
    {
        SceneGameData sceneGameData = new SceneGameData();

        return sceneGameData;
    }
    public void ApplyGameData(SceneGameData sceneGameData) // �ҷ�����
    {
        
    }

    
}


