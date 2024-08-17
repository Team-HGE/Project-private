using Cinemachine;
using UnityEngine;

public class Day_2_SceneInitializer : MonoBehaviour
{
    private void Awake()
    {
        EventManager.Instance.SetSwitch(GameSwitch.DoorUnlocked, false);
        GameManager.Instance.lightManager.elementsForFloors.Clear();
        FloorInitializer.Instance.SetInitializerNull();

        if (GameManager.Instance.lightManager.levers.Count > 0)
        {
            GameManager.Instance.lightManager.levers.Clear();
        }
    }

    private void Start()
    {
        GameManager.Instance.jumpScareManager.OffBtn();
        GameManager.Instance.cinemachineManager.mainCamera = mainCamera;
        GameManager.Instance.cinemachineManager.playerVC = playerVC;
        GameManager.Instance.jumpScareManager.flashLight = playerFlashLight;
        GameManager.Instance.jumpScareManager.blackBG = blackBG;

        GameManager.Instance.nowPlayCutScene = false;

        // 2일차 게임 스위치 변경

        EventManager.Instance.InitializeSwitches(); // 지워

        EventManager.Instance.SetSwitch(GameSwitch.OneFloorEndEscape, true);
        EventManager.Instance.SetSwitch(GameSwitch.NowDay2, true);
        DialogueManager.Instance.StartStory(5);
    }

    void OffLobbyLight()
    {
        GameManager.Instance.lightManager.OffListLight(GameManager.Instance.lightManager.GetLightsForFloor(Floor.Lobby));
        GameManager.Instance.lightManager.OffChangeMaterial(GameManager.Instance.lightManager.GetRenderersForFloor(Floor.Lobby));
    }

    [Header("Cinemachine")]
    [SerializeField] CinemachineVirtualCamera playerVC;
    [SerializeField] CinemachineBrain mainCamera;
    [SerializeField] GameObject blackBG;

    [Header("Light")]
    [SerializeField] Light playerFlashLight;
}
