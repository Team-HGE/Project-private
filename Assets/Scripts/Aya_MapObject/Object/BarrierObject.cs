using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierObject : InteractableObject, IInitializeByLoadedData
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
    public void InitializeByData()
    {
        if (EventManager.Instance.GetSwitch(GameSwitch.BarrierIsOpen))
        {
            OpenAni(true);
        }
        else
        {
            CloseAni(true);
        }
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
                obj.OpenAni(false);
            }
        }
        else 
        {
            foreach (var obj in HotelFloorScene_DataManager.Instance.controller.barrierObjects)
            {
                obj.CloseAni(false);
            }
        }
    }
    public void OpenAni(bool fromInit)
    {
        foreach (var close in closeAni)
        {
            close.DOKill();
        }
        foreach (var animation in openAni)
        {
            animation.duration = 10;
            animation.CreateTween(true);
        }
        if (fromInit)
        {
            foreach (var animation in openAni)
            {
                animation.DOComplete();
            }
        }
        else
        {
            alarmAni.CreateTween(true);
            alarmLight.enabled = true;
            alarmSound.Play();
        }
        isOpen = true;
        EventManager.Instance.SetSwitch(GameSwitch.BarrierIsOpen, true);
    }

    public void CloseAni(bool fromInit)
    {
        foreach (var open in openAni)
        {
            open.DOKill();
        }
        foreach (var animation in closeAni)
        {
            animation.duration = 45;
            animation.CreateTween(true);
        }
        if (fromInit)
        {
            foreach (var animation in closeAni)
            {
                animation.DOComplete();
            }
        }
        else
        {
            alarmAni.CreateTween(true);
            alarmLight.enabled = true;
            alarmSound.Play();
        }
        isOpen = false;
        EventManager.Instance.SetSwitch(GameSwitch.BarrierIsOpen, false);
    }
    public void alaramLightOff()
    {
        alarmSound.Stop();
        alarmLight.enabled = false;
    }

}
