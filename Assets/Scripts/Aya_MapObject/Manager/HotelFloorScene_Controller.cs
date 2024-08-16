using System.Collections.Generic;
using UnityEngine;

public class HotelFloorScene_Controller : MonoBehaviour
{
    public bool isCentralPowerActive { get; set; }
    public bool hasSecurityCard { get; set; }

    public List<DoorObject> doorObjects = new List<DoorObject>();
    public List<BarrierObject> barrierObjects = new List<BarrierObject>();
}
