using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Authentication_Device : InteractableObject
{
    [SerializeField] DOTweenAnimation doorOpen;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material material;
    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = 
        HotelFloorScene_DataManager.Instance.controller.hasSecurityCard ? "[1/1] 보안카드" : "[0/1] 보안카드";
    }
    public override void Interact()
    {
        if (isInteractable) return;
        if (HotelFloorScene_DataManager.Instance.controller.hasSecurityCard)
        {
            doorOpen.CreateTween(true);
            ChangeMaterial();
            isInteractable = true;
        }
    }

    public void ChangeMaterial()
    {
        Material[] newMaterial = meshRenderer.materials;
        newMaterial[1] = material;
        meshRenderer.materials = newMaterial;
    }
}
