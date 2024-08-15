using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerGameData
{
    public Vector3 position; // �÷��̾� ��ġ ����
    public Quaternion rotation; // �÷��̾� ȸ�� ����
}
public class PlayerGameDataManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    public PlayerGameData GetData() // ���̺�
    {
        PlayerGameData playerData = new PlayerGameData();

        {
            playerData.position = transform.position;
            playerData.rotation = transform.rotation;
        };

        return playerData;
    }

    public void ApplyGameData(PlayerGameData playerData)  // �ҷ�����
    {
        playerTransform.position = playerData.position;
        playerTransform.rotation = playerData.rotation;
    }
}
