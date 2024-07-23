using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DrawerObject : InteractableObject
{
    [SerializeField] Vector3 openRotate;
    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "¿­±â";
    }

    public override void Interact()
    {
        isInteractable = true;

        Vector3 target = openRotate;

        transform.DOLocalMove(target , 2f);
    }
}
