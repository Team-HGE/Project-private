using System.Collections.Generic;
using UnityEngine;

public class Day_1_Night_DoorLockEvent : MonoBehaviour
{
    [SerializeField] private GameObject triggerObj;
    [SerializeField] private bool isTrigger;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isTrigger = true;

            AddEvent();
        }
    }
    public void AddEvent()
    {
        GameManager.Instance.fadeManager.EventActionClear();
        GameManager.Instance.fadeManager.fadeComplete += LockAllDoor;
    }
    public void LockAllDoor()
    {
        //NPC 위치 변경
        NPCPos.Instance.SetNightNPCpos();

        foreach (var door in HotelFloorScene_DataManager.Instance.controller.doorObjects)
        {
            if (door.gameObject.CompareTag("NoLock")) { continue; }
            door.isLock = true;
        }

        EventManager.Instance.SetSwitch(GameSwitch.BarrierInteract, true);
        LightSetting();
    }

    public void LightSetting()
    {
        List<Light> lightsA = GameManager.Instance.lightManager.GetLightsForFloor(Floor.AFloor1F);

        foreach (var light in lightsA)
        {
            light.intensity = 7;
        }

        List<Light> lightsB = GameManager.Instance.lightManager.GetLightsForFloor(Floor.BFloor1F);

        foreach (var light in lightsB)
        {
            light.intensity = 5;
        }

        EventManager.Instance.sceneEventManager.DestroyEvent(this, triggerObj);
    }
}
