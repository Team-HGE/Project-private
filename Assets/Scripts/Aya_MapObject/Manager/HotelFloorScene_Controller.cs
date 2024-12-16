using System.Collections.Generic;
using UnityEngine;

public class HotelFloorScene_Controller : MonoBehaviour
{
    public List<DoorObject> doorObjects = new List<DoorObject>();
    public List<BarrierObject> barrierObjects = new List<BarrierObject>();

    private int _computerPassward = 0;
    public int ComputerPassward
    {
        get { return _computerPassward; }
        set 
        { 
            _computerPassward = value;
            if ( _computerPassward == 4)
            {
                EventManager.Instance.SetSwitch(GameSwitch.Day2GetPasswordHint, true);
            }
        }
    }
}
