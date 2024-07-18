using System;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager Instance { get; private set; }

    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    private static Queue<Action> jobQueue = new Queue<Action>();

    private void Awake()
    {
        Debug.Log("StageManager Awake");
        if (Instance == null)
        {
            Instance = this;
            ProcessJobQueue();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void AddEnemy(GameObject enemy)
    {
        if (Instance == null)
        {
            // Awake가 호출되기 전에 함수가 호출된 경우 Job Queue에 추가
            jobQueue.Enqueue(() => Instance.AddEnemyInternal(enemy));
        }
        else
        {
            // 인스턴스가 초기화된 상태라면 즉시 실행
            Instance.AddEnemyInternal(enemy);
        }
    }

    private void AddEnemyInternal(GameObject enemy)
    {
        enemies.Add(enemy);
        Debug.Log("Enemy added. Total enemies: " + enemies.Count);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemies.Remove(enemy);
        Debug.Log("Enemy removed. Total enemies: " + enemies.Count);
    }

    private void ProcessJobQueue()
    {
        // Job Queue가 빌 때까지 Job Queue에 있는 작업을 빼내서 실행
        while (jobQueue.Count > 0)
        {
            Action job = jobQueue.Dequeue();
            job();
        }
    }

    public bool IsStageClear()
    {
        return enemies.Count == 0;
    }
}

public class Enemy : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("Enemy Awake");
        // 적을 StageManager에 등록
        StageManager.AddEnemy(gameObject);
    }

    // 적이 죽었을 때 호출되는 메서드
    public void OnEnemyDeath()
    {
        // 적이 죽었을 때 StageManager에서 적을 제거
        StageManager.Instance.RemoveEnemy(gameObject);
    }
}