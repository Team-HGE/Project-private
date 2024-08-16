using Cinemachine;
using UnityEngine;

public class HotelFloorScene_DataManager : MonoBehaviour
{
    private static HotelFloorScene_DataManager _instance;
    public static HotelFloorScene_DataManager Instance
    {
        get
        {
            _instance ??= FindObjectOfType<HotelFloorScene_DataManager>();
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;
        elevatorManager ??= GetComponent<ElevatorManager>();

        controller ??= GetComponent<HotelFloorScene_Controller>();

        if (playerObj == null)
        {
            playerObj = GameObject.FindGameObjectWithTag("Player");
            GameDataSaveLoadManager.Instance.playerGameDataManager.playerTransform = playerObj.transform;
        }

        //GameDataSaveLoadManager.Instance.LoadGameData(0);
    }
    public GameObject playerObj;
    public ElevatorManager elevatorManager;
    public HotelFloorScene_Controller controller;
    [SerializeField] CinemachineVirtualCamera playerVC;
    public CinemachineVirtualCamera GetPlayerVC { get { return playerVC; } }

    [SerializeField] Transform[] npc_Transforms;
    public Transform[] GetNPC_Transform()
    {
        return npc_Transforms;
    }
}
