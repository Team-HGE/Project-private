using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayTwoStoryInit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // 2���� ���� ����ġ ����
        EventManager.Instance.SetSwitch(GameSwitch.NowDay2, true);

        //GameManager.Instance.Day2Loading();

        StartCoroutine(GameManager.Instance.Day2Loading());

        //DialogueManager.Instance.StartStory(6);
    }


}
