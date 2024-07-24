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
            // Awake�� ȣ��Ǳ� ���� �Լ��� ȣ��� ��� Job Queue�� �߰�
            jobQueue.Enqueue(() => Instance.AddEnemyInternal(enemy));
        }
        else
        {
            // �ν��Ͻ��� �ʱ�ȭ�� ���¶�� ��� ����
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
        // Job Queue�� �� ������ Job Queue�� �ִ� �۾��� ������ ����
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
        // ���� StageManager�� ���
        StageManager.AddEnemy(gameObject);
    }

    // ���� �׾��� �� ȣ��Ǵ� �޼���
    public void OnEnemyDeath()
    {
        // ���� �׾��� �� StageManager���� ���� ����
        StageManager.Instance.RemoveEnemy(gameObject);
    }
}