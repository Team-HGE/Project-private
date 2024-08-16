using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_1_SceneInitializer : MonoBehaviour
{
    [SerializeField] private GameObject blackBG;
    [SerializeField] private Light playerLight;

    [SerializeField] private GameObject eyeTypeMonser;
    [SerializeField] private GameObject groupTypeMonser;

    [SerializeField] private CinemachineBlendListCamera blendListCamera;
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
   
    private void Awake()
    {
        GameManager.Instance.lightManager.elementsForFloors.Clear();
        FloorInitializer.Instance.SetInitializerNull();

        if (GameManager.Instance.lightManager.levers.Count > 0)
        {
            GameManager.Instance.lightManager.levers.Clear();
        }
    }
    private void Start()
    {
        GameManager.Instance.cinemachineManager.playerVC = virtualCamera;
        GameManager.Instance.cinemachineManager.blendListCamera = blendListCamera;  

        GameManager.Instance.jumpScareManager.blackBG = blackBG;
        GameManager.Instance.jumpScareManager.flashLight = playerLight;
        GameManager.Instance.jumpScareManager.monstersJumpScare[0].gameObject = groupTypeMonser;
        //GameManager.Instance.jumpScareManager.monstersJumpScare[1].gameObject = eyeTypeMonser;

        DialogueManager.Instance.StartStory(1);
        EventManager.Instance.SetSwitch(GameSwitch.isCentralPowerActive, true);
    }
}
