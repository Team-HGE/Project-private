using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_2_SceneManager : MonoBehaviour
{
    private static Day_2_SceneManager _instance;
    public static Day_2_SceneManager Instance
    {
        get
        {
            _instance ??= FindObjectOfType<Day_2_SceneManager>();
            return _instance;
        }
    }

    private void Awake()
    {
        if (GameManager.Instance.lightManager.lavers.Count > 0)
        GameManager.Instance.lightManager.lavers.Clear();
    }

    private void Start()
    {
        StartCoroutine(FloorLightOff());
    }

    IEnumerator FloorLightOff()
    {
        yield return new WaitForSeconds(1);
        while (true)
        {
            if (GameManager.Instance.lightManager.lavers.Count > 0)
            {
                GameManager.Instance.lightManager.OffLaversAllLight();
                StopCoroutine(FloorLightOff());
            }
        }
    }
}
