using Cinemachine;
using UnityEngine;

public class HotelFloorScene_DataManager : MonoBehaviour
{
    public static HotelFloorScene_DataManager Instance;
    private void Awake()
    {
        if (Instance == null) Instance = this;

        if (elevatorManager == null) elevatorManager = GetComponent<ElevatorManager>();

        if (controller == null) controller = GetComponent<HotelFloorScene_Controller>();
    }
    public ElevatorManager elevatorManager;
    public HotelFloorScene_Controller controller;
    [SerializeField] CinemachineVirtualCamera playerVC;
    public CinemachineVirtualCamera GetPlayerVC { get { return playerVC; } }
}
