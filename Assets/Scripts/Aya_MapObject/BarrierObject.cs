using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrierObject : InteractableObject
{
    [SerializeField] DOTweenAnimation[] openAni;
    [SerializeField] DOTweenAnimation[] closeAni;
    [SerializeField] Light alarmLight;
    [SerializeField] DOTweenAnimation alarmAni;
    [SerializeField] bool isOpen { get; set; }
    float time;
    private void Start()
    {
        isOpen = true;
        HotelFloorScene_DataManager.Instance.controller.barrierObjects.Add(this);
    }
    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "차단벽 해제";
    }
    public override void Interact()
    {
        if (isInteractable) return;
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
            animation.CreateTween(true);
            alarmAni.CreateTween(true);
            alarmLight.enabled = true;
            isOpen = true;
        }
    }
    public void CloseAni()
    {
        foreach (var open in openAni)
        {
            open.DOKill();
        }
        foreach (var animation in closeAni)
        {
            animation.CreateTween(true);
            alarmAni.CreateTween(true);
            alarmLight.enabled = true;
            isOpen = false;
        }
    }
    public void alaramLightOff()
    {
        alarmLight.enabled = false;
    }
}
