using TMPro;
using UnityEngine;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] TextMeshProUGUI interactableText;

    public void ActivateInteraction()
    {
        //Message
        //+= Interact;
    }
    public void Interact()
    {

    }
}
