using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[Serializable]
public class PlayerGameData
{
    public PlayerGameData()
    {
        position = new SerializableVecter3();
        rotation = new SerializableVecter3();
    }
    public SerializableVecter3 position; // �÷��̾� ��ġ ����
    public SerializableVecter3 rotation; // �÷��̾� ȸ�� ����
}

[System.Serializable]
public class SerializableVecter3
{
    public float x;
    public float y;
    public float z;

    public SerializableVecter3(Vector3 vector3)
    {
        SetVector(vector3);
    }
    public void SetVector(Vector3 vector3)
    {
        x = vector3.x;
        y = vector3.y;
        z = vector3.z;
    }

    public Vector3 GetVector()
    {
        return new Vector3(x, y, z);
    }
    public SerializableVecter3() { }
}
public class PlayerGameDataManager : MonoBehaviour
{
    public Transform playerTransform;
    
    public PlayerGameData GetData() // ���̺�
    {
        PlayerGameData playerData = new PlayerGameData();

        playerTransform = GameManager.Instance.player.transform;

        playerData.position.SetVector(playerTransform.position);
        playerData.rotation.SetVector(playerTransform.rotation.eulerAngles);
        return playerData;
    }

    public void ApplyGameData(PlayerGameData playerData)  // �ҷ�����
    {
        playerTransform = GameManager.Instance.player.transform;

        playerTransform.position = playerData.position.GetVector();
        playerTransform.rotation = Quaternion.Euler(playerData.rotation.GetVector());
    }
}
