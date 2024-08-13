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
                //Debug.Log("노이즈 풀 오브젝트 널");

                _instance = FindObjectOfType(typeof(NoisePool)) as NoisePool;
            }
            return _instance;
        }

    }

    // 외부의 오브젝드에서 여기에 오디오 클립을 담은 리스트를 전달
    public List<NoiseData> noiseDatasList;

    public Dictionary<string, List<GameObject>> poolDictionary;

    //public void Initialize()
    //{
    //    //Debug.Log($"NoisePool - Initialize");
    //    if (noiseDatasList == null)
    //    {
    //        Debug.Log($"NoisePool - Initialize -  noiseDatasList null, 초기화");
    //        noiseDatasList = new List<NoiseData>();
    //    }
    //    else
    //    {
    //        Debug.Log($"NoisePool - Initialize -  noiseDatasList 있음 {noiseDatasList.Count}");

    //    }
    //}

    public void FindNoise()
    {
        //if (noiseDatasList.Count == 0)
        //{
        //    Debug.Log("노이즈 0개");
        //}
        //Debug.Log($"노이즈 종류 갯수 : {noiseDatasList.Count}");


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

        // 미리 만든 프리펩이 있을 때
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeSelf)
            {
                obj = list[i];
                break;
            }
        }

        // 없을 때
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
