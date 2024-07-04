using UnityEngine;
using DG.Tweening;

public class DoorObject : InteractableObject
{
    [SerializeField] bool isOpen;
    [SerializeField] bool interactableOneTime;
    [SerializeField] Vector3 openRotate;
    [SerializeField] Vector3 closeRotate;
    [SerializeField] Transform parentObject;
    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "[E] DoorOpen";
    }

    public override void Interact()
    {
        if (interactableOneTime)
        {
            isInteractable = true;
        }

        Vector3 targetRotate;
        if (isOpen )
        {
            targetRotate = closeRotate;
            isOpen = false;
        }
        else
        {
            targetRotate = openRotate;
            isOpen = true;
        }
        parentObject.DOKill();
        parentObject.DORotate(targetRotate, 2f);
    }
}
