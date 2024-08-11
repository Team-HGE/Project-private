using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstCutSceneEvent : MonoBehaviour
{
    [Header("First")]
    [SerializeField] MeshRenderer[] firstRenderer;
    [SerializeField] Light[] firstLight;

    [Header("Second")]
    [SerializeField] MeshRenderer[] secondRenderer;
    [SerializeField] Light[] secondLight;
    
    [Header("Third")]
    [SerializeField] MeshRenderer[] thirdRenderer;
    [SerializeField] Light[] thirdLight;

    WaitForSeconds waitTime = new WaitForSeconds(1.5f);

    [Header("And")]

    [SerializeField] AudioSource audioSource;
    public Laver floorB_Laver;
    void LightOff(Light[] offLights)
    {
        foreach (Light light in offLights)
        {
            light.enabled = false;
        }
    }

    public void EventOn()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        StartCoroutine(LightSystem());
    }

    IEnumerator LightSystem()
    {
        LightOff(firstLight);
        GameManager.Instance.lightManager.OffChangeMaterial(firstRenderer);
        audioSource.Play();

        yield return waitTime;

        LightOff(secondLight);
        GameManager.Instance.lightManager.OffChangeMaterial(secondRenderer);
        audioSource.Play();

        yield return waitTime;

        LightOff(thirdLight);
        GameManager.Instance.lightManager.OffChangeMaterial(thirdRenderer);
        audioSource.Play();

        yield return waitTime;

        floorB_Laver.OffNowFloorAllLight();
        foreach(var obj in HotelFloorScene_DataManager.Instance.controller.barrierObjects)
        {
            obj.CloseAni();
            obj.isInteractable = true;
        }
        Destroy(gameObject);
    }
}
