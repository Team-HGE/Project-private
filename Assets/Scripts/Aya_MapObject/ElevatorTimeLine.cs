﻿using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class ElevatorTimeLine : MonoBehaviour
{
    public bool triggerOn; 
    public PlayableDirector timelineDirector;
    public GameObject timeLineObject;
    public GameObject[] UI;
    public Light playerLight;

    public TextMeshPro roomTxt101;
    public TextMeshPro roomTxt102;
    public TextMeshPro roomTxt101_111;
    public TextMeshPro roomTxt112_121;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !triggerOn)
        {
            triggerOn = true;
            GameManager.Instance.fadeManager.fadeComplete += ElevatorMovie;
            StartCoroutine(GameManager.Instance.fadeManager.FadeStart(FadeState.FadeOut));
        }
    }

    private void ElevatorMovie()
    {
        timeLineObject.SetActive(true);
        playerLight.enabled = false;
        foreach (GameObject go in UI)
        {
            go.SetActive(false);
        }

        GameManager.Instance.nowPlayCutScene = true;

        timelineDirector.Play();
        GameManager.Instance.fadeManager.fadeComplete -= ElevatorMovie;
    }

    public void OffLight()
    {
        GameManager.Instance.lightManager.OffLaversAllLight();
    }

    public void ChangeTxt()
    {
        roomTxt101.text = "2";
        roomTxt102.text = "2";
        roomTxt101_111.text = "201 - 211";
        roomTxt112_121.text = "212 - 221";
    }

    public void FadeOff()
    {
        StartCoroutine(GameManager.Instance.fadeManager.FadeStart(FadeState.FadeIn));
    }
    public void ChangeScene()
    {
        GameManager.Instance.nowPlayCutScene = false;

        GameManager.Instance.fadeManager.MoveScene(SceneEnum.Hotel_Day2);
        Invoke("SetUI", 1.5f);
    }
    void SetUI()
    {
        foreach (GameObject go in UI)
        {
            go.SetActive(true);
        }
    }
}
