using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Laver : InteractableObject
{
    [Header("OnOffLight")]
    [SerializeField] Light[] nowFloorlights;
    [SerializeField] MeshRenderer[] nowFloorObjectRenderers;
    [SerializeField] GameObject[] nowFloorObject;

    [Header("ObjectControll")]
    [SerializeField] DOTweenAnimation laverUp;
    private void Start()
    {
        for (int i = 0; i < GameManager.Instance.lightManager.lavers.Length; i++)
        {
            if (GameManager.Instance.lightManager.lavers[i] == null)
            {
                GameManager.Instance.lightManager.lavers[i] = this;
                break;
            }
        }
    }
    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "¿Ã¸®±â";
    }
    public override void Interact()
    {
        if (isInteractable) return;
        laverUp.DOPlay();
        isInteractable = true;
    }
    public void OffNowFloorAllLight()
    {
        foreach (var light in nowFloorlights)
        {
            light.enabled = false;
        }
        foreach (var obj in nowFloorObject)
        {
            obj.SetActive(false);
        }

        GameManager.Instance.lightManager.ChangeMaterial(nowFloorObjectRenderers);
    }
    public void OnNowFloorAllLight()
    {
        foreach (var light in nowFloorlights)
        {
            light.enabled = true;
        }
        foreach (var obj in nowFloorObject)
        {
            obj.SetActive(true);
        }

        GameManager.Instance.lightManager.ChangeMaterial(nowFloorObjectRenderers);
    }
}
