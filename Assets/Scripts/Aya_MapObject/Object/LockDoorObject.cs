using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorObject : InteractableObject
{
    [Header("InteractToggle")]
    public bool onInteract;

    [Header("DotweenAnimation")]
    [SerializeField] DOTweenAnimation doorOpen;

    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        if (!onInteract) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "¿­±â";
    }
    public override void Interact()
    {
        if (isInteractable) return;
        if (!onInteract) return;

        isInteractable = true;

        doorOpen.DOPlay();
    }
}
