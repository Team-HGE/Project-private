using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class SceneEventManager : MonoBehaviour
{
    public event Action playEvent;

    [TabGroup("Tab", "Scripts", SdfIconType.GearFill, TextColor = "blue")]

    public void EvnetActionClear()
    {
        playEvent = null;
    }

    public void StartEvent()
    {
        playEvent?.Invoke();
        playEvent = null;
    }
    public void DestroyEvent(MonoBehaviour eventScript, GameObject obj)
    {
        if (obj != null)
        {
            Destroy(obj);
        }
        Destroy(eventScript);
    }
}
