using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayOneStoryInit : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(GameManager.Instance.Day1Loading());

    }
}
