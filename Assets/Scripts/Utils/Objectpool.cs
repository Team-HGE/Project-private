using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ObjectPool : MonoBehaviour
{
    public GameObject prefab;
    public Transform parrent;
    public int poolSize = 10;

    private List<GameObject> pool;

    private void Awake()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj;
            if(parrent == null)
            {
                obj = Instantiate(prefab);

            }
            else
            {
                obj = Instantiate(prefab, parrent);
            }
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    public GameObject GetObject()
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(true);
                return obj;
            }
        }

        GameObject newObj = Instantiate(prefab);
        newObj.SetActive(true);
        pool.Add(newObj);
        return newObj;
    }

    public void ReturnObject(GameObject obj)
    {
        obj.SetActive(false);
    }

    public void ReturnObjectbyIndex(int index)
    {
        pool[0].SetActive(false);
        for (int i = 1; i < index-1; i++)
        {
            pool[i] = pool[i + 1];
        }
        //pool[index-1].SetActive(false);
    }

    public void ReturnAllObject()
    {
        for (int i = 0; i < poolSize; i++)
        {
            pool[i].SetActive(false);
        }
        Debug.Log("풀 초기화 완료");
    }
}
