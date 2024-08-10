using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCutSceneEvent : MonoBehaviour
{
    [SerializeField] Light[] offLights;
    WaitForSeconds waitTime1 = new WaitForSeconds(0.3f);
    WaitForSeconds waitTime2 = new WaitForSeconds(2f);
    WaitForSeconds waitTime3 = new WaitForSeconds(5f);

    public Laver floorB_Laver;
    void LightOff()
    {
        foreach (Light light in offLights)
        {
            light.enabled = false;
        }
    }
    void LightOn()
    {
        foreach (Light light in offLights)
        {
            light.enabled = true;
        }
    }

    public void EventOn()
    {
        StartCoroutine(LightSystem());
    }

    IEnumerator LightSystem()
    {
        LightOff();
        yield return waitTime1;
        LightOn();
        yield return waitTime1;
        LightOff();
        yield return waitTime1;
        LightOn();
        yield return waitTime2;
        LightOff();
        yield return waitTime2;
        floorB_Laver.OffNowFloorAllLight();
        foreach(var obj in HotelFloorScene_DataManager.Instance.controller.barrierObjects)
        {
            obj.CloseAni();
            obj.isInteractable = true;
        }
        Destroy(gameObject);
    }
}
