using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_1_SceneInitializer : MonoBehaviour
{
    private void Start()
    {
        DialogueManager.Instance.StartStory(1);
        EventManager.Instance.SetSwitch(GameSwitch.isCentralPowerActive, true);
    }
}
