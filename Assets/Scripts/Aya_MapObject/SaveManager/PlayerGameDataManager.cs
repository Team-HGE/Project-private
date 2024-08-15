using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PlayerGameData
{
    public Vector3 position; // 플레이어 위치 정보
    public Quaternion rotation; // 플레이어 회전 정보
}
public class PlayerGameDataManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;

    public PlayerGameData GetData() // 세이브
    {
        PlayerGameData playerData = new PlayerGameData();

        {
            playerData.position = transform.position;
            playerData.rotation = transform.rotation;
        };

        return playerData;
    }

    public void ApplyGameData(PlayerGameData playerData)  // 불러오기
    {
        playerTransform.position = playerData.position;
        playerTransform.rotation = playerData.rotation;
    }
}
