using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractable : MonoBehaviour
{
    [Header("public")]
    public GameObject curInteractableGameObject;
    public TextMeshProUGUI interactableText;

    [Header("SerializeField")]
    [SerializeField] float interactionRange = 2.0f;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Camera camera;

    IInteractable curInteractable;
    float checkRate = 0.05f;
    float lastCheckTime;

    private void Start()
    {
        camera = GetComponent<Camera>();
        camera = Camera.main;
    }
    void Update()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            InteractWithObject();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
        Debug.DrawRay(transform.position, transform.forward, Color.red, interactionRange);
    }

    void InteractWithObject()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactionRange, layerMask))
        {
            if (hit.collider.gameObject != curInteractableGameObject)
            {
                curInteractableGameObject = hit.collider.gameObject;
                curInteractable = hit.collider.gameObject.GetComponent<IInteractable>();
                curInteractable.ActivateInteraction();
            }
        }
        else
        {
            curInteractableGameObject = null;
            curInteractable = null;
            interactableText.gameObject.SetActive(false);
        }
    }

    public void OnInteracted(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && curInteractable != null && !curInteractable.isInteractable)
        {
            curInteractable.Interact();
            curInteractableGameObject = null;
            curInteractable = null;
            interactableText.gameObject.SetActive(false);
        }
    }

    void Interact()
    {
        if (curInteractable != null && !curInteractable.isInteractable)
        {
            curInteractable.Interact();
            curInteractableGameObject = null;
            curInteractable = null;
            interactableText.gameObject.SetActive(false);
        }
    }
}
