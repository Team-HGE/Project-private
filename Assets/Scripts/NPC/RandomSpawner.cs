using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public List<GameObject> objectsToSpawn; // 스폰할 게임 오브젝트의 리스트

    [Header("Spawn Range")]
    public float minX = -80f;
    public float maxX = -25f;
    public float minZ = 65f;
    public float maxZ = 140f;
    public float fixedY = 0f;

    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        if (objectsToSpawn.Count == 0)
        {
            Debug.LogError("스폰할 오브젝트 목록이 비어 있습니다.");
            return;
        }

        SpawnObjects();
    }

    void SpawnObjects()
    {
        for (int i = 0; i < objectsToSpawn.Count; i++)
        {
            Vector3 spawnPosition = GenerateRandomPosition();
            while (spawnedPositions.Contains(spawnPosition))
            {
                spawnPosition = GenerateRandomPosition();
            }

            GameObject objectToSpawn = objectsToSpawn[i];
            Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
            spawnedPositions.Add(spawnPosition);
        }
    }

    Vector3 GenerateRandomPosition()
    {
        float randomX = Random.Range(minX, maxX);
        float randomZ = Random.Range(minZ, maxZ);
        return new Vector3(randomX, fixedY, randomZ);
    }
}
