using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

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

    public GameObject ReturnObjectby(Sprite sprite)
    {
        for (int i = 0; i < poolSize; i++)
        {
            if(sprite == pool[i].GetComponent<Image>().sprite)
            {
                return pool[i];
            }
        }
        return null;
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
