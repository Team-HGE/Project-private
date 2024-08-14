using UnityEngine;

public class Day_1_Night_DoorLockEvent : MonoBehaviour
{
    public void EventOn()
    {
        GameManager.Instance.fadeManager.EventActionClear();
        GameManager.Instance.fadeManager.fadeComplete += LockAllDoor;
    }
    public void LockAllDoor()
    {
        foreach (var door in HotelFloorScene_DataManager.Instance.controller.doorObjects)
        {
            if (door.gameObject.CompareTag("NoLock")) { continue; }
            door.isLock = true;
        }
        EventManager.Instance.sceneEventManager.DestroyEvent(this);
    }
}
