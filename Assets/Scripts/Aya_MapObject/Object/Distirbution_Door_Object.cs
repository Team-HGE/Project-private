using DG.Tweening;
using UnityEngine;

public class Distirbution_Door_Object : InteractableObject
{
    [SerializeField] DOTweenAnimation openDoor;
    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "¿­±â";
    }
    public override void Interact()
    {
        if (isInteractable) return;
        isInteractable = true;

        openDoor.DOPlay();
    }
}
