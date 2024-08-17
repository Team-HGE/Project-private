using Cinemachine;
using Sirenix.OdinInspector;
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
        DialogueManager.Instance.set.ui.playEvent += Day2Save;
        DialogueManager.Instance.StartStory(5);

        PSYNpc.npcEvent += SetDay2PasswordHint;
    }
    void Day2Save()
    {
        Debug.Log("세이브 성공");
        //GameDataSaveLoadManager.Instance.SaveGameData(0);
    }
    void OffLobbyLight()
    {
        GameManager.Instance.lightManager.OffListLight(GameManager.Instance.lightManager.GetLightsForFloor(Floor.Lobby));
        GameManager.Instance.lightManager.OffChangeMaterial(GameManager.Instance.lightManager.GetRenderersForFloor(Floor.Lobby));
    }
    void SetDay2PasswordHint()
    {
        if (PSYNpc.nPC_Name == NPC_Name.PSY)
        {
            DialogueManager.Instance.npcScript.playEvent += Day2PasswordHint1;
        }
    }
    void Day2PasswordHint1()
    {
        HotelFloorScene_DataManager.Instance.controller.ComputerPassward += 1;
    }


    [Header("Cinemachine")]
    [SerializeField] private CinemachineVirtualCamera playerVC;
    [SerializeField] private CinemachineBrain mainCamera;
    [SerializeField] private GameObject blackBG;

    [Header("Light")]
    [SerializeField] private Light playerFlashLight;

    [Title("NPC")]
    [SerializeField] private NPC PSYNpc;
}
