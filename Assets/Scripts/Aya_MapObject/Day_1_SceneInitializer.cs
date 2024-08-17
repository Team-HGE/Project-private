using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Day_1_SceneInitializer : MonoBehaviour
{
    [SerializeField] private GameObject blackBG;
    [SerializeField] private Light playerLight;

    [SerializeField] private GameObject eyeTypeMonster;
    [SerializeField] private GameObject groupTypeMonster;

    [SerializeField] private CinemachineBrain mainCamera;
    [SerializeField] private CinemachineVirtualCamera playerVC;
   
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
        // 플레이어 켄버스 세팅
        GameManager.Instance.playerInteractionCanvas.SetActive(true);
        GameManager.Instance.crossHairCanvas.SetActive(true);
        GameManager.Instance.circleUI.SetActive(true);
        GameManager.Instance.timeUI.SetActive(true);

        // 스위치 세팅
        GameManager.Instance.dayNightUI.UpdateDayNightUI(EventManager.Instance.GetSwitch(GameSwitch.IsDaytime));
        EventManager.Instance.SetSwitch(GameSwitch.IsDaytime, true);
        EventManager.Instance.SetSwitch(GameSwitch.isCentralPowerActive, true);

        // 시네머신 세팅
        GameManager.Instance.cinemachineManager.playerVC = playerVC;
        GameManager.Instance.cinemachineManager.mainCamera = mainCamera;  

        // 점프스케어 세팅
        GameManager.Instance.jumpScareManager.blackBG = blackBG;
        GameManager.Instance.jumpScareManager.flashLight = playerLight;
        GameManager.Instance.jumpScareManager.monstersJumpScare[0].gameObject = groupTypeMonster;
        GameManager.Instance.jumpScareManager.monstersJumpScare[1].gameObject = eyeTypeMonster;

        // 다이얼로그 세팅
        DialogueManager.Instance.StartStory(1);
       
    }
}
