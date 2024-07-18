using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NoisePool : MonoBehaviour
{
    private static NoisePool _instance;

    public static NoisePool Instance
    {
        get
        {
            if (_instance == null)
            {
                //Debug.Log("������ Ǯ ������Ʈ ��");

                _instance = FindObjectOfType(typeof(NoisePool)) as NoisePool;
            }
            return _instance;
        }

    }

    // �ܺ��� �������忡�� ���⿡ ����� Ŭ���� ���� ����Ʈ�� ����
    public List<NoiseData> noiseDatasList;

    public Dictionary<string, List<GameObject>> poolDictionary;

    public void Initialize()
    {
        //Debug.Log($"NoisePool - Initialize");

        noiseDatasList = new List<NoiseData>();

        if (_instance == null)
        {
            Debug.Log($"NoisePool - Initialize -  null");
            _instance = this;
        }
    }

    public void FindNoise()
    {
        //if (noiseDatasList.Count == 0)
        //{
        //    Debug.Log("������ 0��");
        //}
        //Debug.Log($"������ ���� ���� : {noiseDatasList.Count}");


        poolDictionary = new Dictionary<string, List<GameObject>>();

        for (int i = 0; i < noiseDatasList.Count; i++)
        {
            List<GameObject> noiseDatas = new List<GameObject>();
            poolDictionary.Add(noiseDatasList[i].tag, noiseDatas);

        }

        //foreach (KeyValuePair<string, List<GameObject>> item in poolDictionary)
        //{
        //    Debug.Log($"tag : {item.Key}, value : {item.Value.Count}");
        //}
    }

    public GameObject SpawnFromPool(string tag)
    {
        if (!poolDictionary.ContainsKey(tag)) return null;

        List<GameObject> list = poolDictionary[tag];
        GameObject obj = null;

        // �̸� ���� �������� ���� ��
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeSelf)
            {
                obj = list[i];
                break;
            }
        }

        // ���� ��
        if (!obj)
        {
            NoiseData temp;

            foreach (NoiseData noiseData in noiseDatasList)
            {
                if (noiseData.tag == tag)
                {
                    temp = noiseData;
                    obj = Instantiate(temp.prefab, temp.box);
                }
            }

            foreach (KeyValuePair<string, List<GameObject>> item in poolDictionary)
            {
                if (item.Key == tag)
                {
                    item.Value.Add(obj);
                }
            }
        }

        obj.SetActive(true);
        return obj;
    }
}
