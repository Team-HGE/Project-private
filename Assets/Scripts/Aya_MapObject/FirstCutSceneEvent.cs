using Sirenix.OdinInspector;
using System.Collections;
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
    public Lever floorB_Laver;

    [Title("NPC")]
    [SerializeField] private GameObject npc;
    void LightOff(Light[] offLights)
    {
        foreach (Light light in offLights)
        {
            light.enabled = false;
        }
    }

    [Header("Monsters")]
    [SerializeField] GameObject SM2;


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

        JOE_Spawn();

        SM2.SetActive(true);

        Destroy(gameObject);
    }

    void JOE_Spawn()
    {
        npc.SetActive(true);
    }
}
