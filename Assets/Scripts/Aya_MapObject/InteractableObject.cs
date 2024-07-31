using UnityEngine;

public abstract class InteractableObject : MonoBehaviour, IInteractable
{
    public bool isInteractable { get ; set ;}

    public abstract void ActivateInteraction();

    public abstract void Interact();
}
