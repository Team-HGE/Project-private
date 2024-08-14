using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class SceneEventManager : MonoBehaviour
{
    public event Action playEvent;

    [TabGroup("Tab", "Scripts", SdfIconType.GearFill, TextColor = "blue")]
    [TabGroup("Tab", "Scripts" )] [SerializeField] private Day_1_Night_DoorLockEvent day_1_Night_DoorLockEvent;

    private void Start()
    {
        if (day_1_Night_DoorLockEvent == null) day_1_Night_DoorLockEvent = GetComponent<Day_1_Night_DoorLockEvent>();
    }

    public void EvnetActionClear()
    {
        playEvent = null;
    }

    public void StartEvent()
    {
        playEvent?.Invoke();
        playEvent = null;
    }
    public void DestroyEvent(MonoBehaviour eventScript)
    {
        Destroy(eventScript);
    }
}
