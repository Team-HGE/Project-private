using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierObject : InteractableObject
{
    [Header("Ani")]
    [SerializeField] DOTweenAnimation[] openAni;
    [SerializeField] DOTweenAnimation[] closeAni;
    [SerializeField] DOTweenAnimation alarmAni;

    [Header("Alarm")]
    [SerializeField] GameObject alram;
    [SerializeField] AudioSource alarmSound;

    [SerializeField] Light alarmLight;

    [Header("Bool Check")]
    [SerializeField] bool _isOpen;
    public bool isOpen
    {
        get { return _isOpen; }
        set { _isOpen = value; } 
    }
    float time;
    private void Start()
    {
        HotelFloorScene_DataManager.Instance.controller.barrierObjects.Add(this);
        alarmSound = alram.GetComponent<AudioSource>();
    }
    public override void ActivateInteraction()
    {
        if (!EventManager.Instance.GetSwitch(GameSwitch.BarrierInteract) || !HotelFloorScene_DataManager.Instance.controller.isCentralPowerActive) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "차단벽 해제";
    }
    public override void Interact()
    {
        if (!EventManager.Instance.GetSwitch(GameSwitch.BarrierInteract) || !HotelFloorScene_DataManager.Instance.controller.isCentralPowerActive) return;
        if (!isOpen)
        {
            foreach (var obj in HotelFloorScene_DataManager.Instance.controller.barrierObjects)
            {
                obj.OpenAni();
            }
        }
        else 
        {
            foreach (var obj in HotelFloorScene_DataManager.Instance.controller.barrierObjects)
            {
                obj.CloseAni();
            }
        }
    }
    public void OpenAni()
    {
        foreach (var close in closeAni)
        {
            close.DOKill();
        }
        foreach (var animation in openAni)
        {
            animation.duration = 10;
            animation.CreateTween(true);
            alarmAni.CreateTween(true);
            alarmLight.enabled = true;
            isOpen = true;
        }
        alarmSound.Play();
    }
    public void CloseAni()
    {
        foreach (var open in openAni)
        {
            open.DOKill();
        }
        foreach (var animation in closeAni)
        {
            animation.duration = 45;
            animation.CreateTween(true);
            alarmAni.CreateTween(true);
            alarmLight.enabled = true;
            isOpen = false;
        }
        alarmSound.Play();
    }
    public void alaramLightOff()
    {
        alarmSound.Stop();
        alarmLight.enabled = false;
    }
}
