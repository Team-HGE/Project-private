using UnityEngine;

public class PlayerInteractable : MonoBehaviour
{
    public float interactionRange = 2.0f;

    void Update()
    {
        InteractWithObject();
        Debug.DrawRay(transform.position, transform.forward, Color.red, interactionRange);
    }

    void InteractWithObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, interactionRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }
}
