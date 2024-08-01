using Cinemachine;
using UnityEngine;

public class HotelFloorScene_DataManager : MonoBehaviour
{
    public static HotelFloorScene_DataManager Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    [SerializeField] CinemachineVirtualCamera playerVC;
    public CinemachineVirtualCamera GetPlayerVC { get { return playerVC; } }
}
