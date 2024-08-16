using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTwoStoryInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 2일차 게임 스위치 변경
        EventManager.Instance.SetSwitch(GameSwitch.NowDay2, true);

        //GameManager.Instance.Day2Loading();

        StartCoroutine(GameManager.Instance.Day2Loading());

        //DialogueManager.Instance.StartStory(6);
    }


}
