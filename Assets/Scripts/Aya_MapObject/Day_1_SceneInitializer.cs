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
        // �÷��̾� �˹��� ����
        GameManager.Instance.playerInteractionCanvas.SetActive(true);
        GameManager.Instance.crossHairCanvas.SetActive(true);
        GameManager.Instance.circleUI.SetActive(true);
        GameManager.Instance.timeUI.SetActive(true);

        // ����ġ ����
        GameManager.Instance.dayNightUI.UpdateDayNightUI(EventManager.Instance.GetSwitch(GameSwitch.IsDaytime));
        EventManager.Instance.SetSwitch(GameSwitch.IsDaytime, true);
        EventManager.Instance.SetSwitch(GameSwitch.isCentralPowerActive, true);

        // �ó׸ӽ� ����
        GameManager.Instance.cinemachineManager.playerVC = playerVC;
        GameManager.Instance.cinemachineManager.mainCamera = mainCamera;  

        // �������ɾ� ����
        GameManager.Instance.jumpScareManager.blackBG = blackBG;
        GameManager.Instance.jumpScareManager.flashLight = playerLight;
        GameManager.Instance.jumpScareManager.monstersJumpScare[0].gameObject = groupTypeMonster;
        GameManager.Instance.jumpScareManager.monstersJumpScare[1].gameObject = eyeTypeMonster;

        // ���̾�α� ����
        DialogueManager.Instance.StartStory(1);
       
    }
}
