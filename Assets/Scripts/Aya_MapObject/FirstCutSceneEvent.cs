using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCutSceneEvent : MonoBehaviour
{
    [SerializeField] Light[] offLights;

    public WaitForSeconds waitTime2 = new WaitForSeconds(2f);
    public WaitForSeconds waitTime1 = new WaitForSeconds(0.3f);

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

    public void SystemOn()
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
        yield return null;
    }
}
