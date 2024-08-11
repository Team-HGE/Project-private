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
        elevatorManager ??= GetComponent<ElevatorManager>();

        controller ??= GetComponent<HotelFloorScene_Controller>();

        GameManager.Instance.lightManager.lobbyLights?.Clear();
    }
    public ElevatorManager elevatorManager;
    public HotelFloorScene_Controller controller;
    [SerializeField] CinemachineVirtualCamera playerVC;
    public CinemachineVirtualCamera GetPlayerVC { get { return playerVC; } }
}
