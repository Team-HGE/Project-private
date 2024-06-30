using UnityEngine;

public class DoorObject : InteractableObject
{
    [SerializeField] Animator animator;
    public override void ActivateInteraction()
    {
        if (isInteractable) return;

        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "[E] DoorOpen";
    }

    public override void Interact()
    {
        isInteractable = true;
        animator = GetComponent<Animator>();
        animator.SetBool("isOpen", true);
    }
}
