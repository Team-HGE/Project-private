﻿using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Authentication_Device : InteractableObject
{
    [SerializeField] DOTweenAnimation doorOpen;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material material;
    [SerializeField] private GameObject glare;
    [Header("SecondDayEvent")]
    public ScriptSO[] scriptSOs;

    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);

        GameManager.Instance.player.interactableText.text =
            EventManager.Instance.GetSwitch(GameSwitch.Day2GetCardKey) ? "[1/1] 보안카드" : "[0/1] 보안카드";
    }

    public override void Interact()
    {
        if (isInteractable) return;

        if (EventManager.Instance.GetSwitch(GameSwitch.Day2GetCardKey))
        {
            glare.SetActive(false);
            doorOpen.CreateTween(true);
            ChangeMaterial();
            isInteractable = true;
            SecondDayEventScript(1);
        }
        else SecondDayEventScript(0);
    }

    public void ChangeMaterial()
    {
        Material[] newMaterial = meshRenderer.materials;
        newMaterial[1] = material;
        meshRenderer.materials = newMaterial;
    }

    public void SecondDayEventScript(int index)
    {
        if (!EventManager.Instance.GetSwitch(GameSwitch.NowDay2) || EventManager.Instance.GetSwitch(GameSwitch.Day2GetCardKey)) return;

        DialogueManager.Instance.itemScript.Init(scriptSOs[index]);
        DialogueManager.Instance.itemScript.Print();
    }
}
