using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterectable : MonoBehaviour
{
    public float interactionRange = 2.0f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithObject();
        }
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
