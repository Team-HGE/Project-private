using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerInteractable : MonoBehaviour
{
    [Header("public")]
    public GameObject curInteractableGameObject;
    public GameObject playerInteraction;
    public TextMeshProUGUI interactableText;
    public Image interactionImage;
    public Image fillAmountImage;
    public float holdTime = 2.0f;

    [Header("SerializeField")]
    [SerializeField] float interactionRange = 2.0f;
    [SerializeField] float holdDuration = 0f;
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
            Debug.Log("objectNull");
            curInteractableGameObject = null;
            curInteractable = null;
            playerInteraction.SetActive(false);
            holdDuration = 0f;
            fillAmountImage.fillAmount = 0f;
        }
    }

    public void OnInteracted()
    {
        Debug.Log("?");
        if (curInteractable != null && !curInteractable.isInteractable)
        {
            holdDuration += Time.deltaTime;
            fillAmountImage.fillAmount = Mathf.Clamp01(holdDuration / holdTime); // 1과 0사이 수 리턴
            if (holdDuration >= holdTime)
            {
                if (curInteractable != null && !curInteractable.isInteractable)
                {
                    curInteractable.Interact();
                    curInteractableGameObject = null;
                    curInteractable = null;
                    playerInteraction.SetActive(false);
                    holdDuration = 0f;
                    fillAmountImage.fillAmount = 0f;
                }
            }
        }
    }
    public void EndInteraction()
    {
        holdDuration = 0f;
        fillAmountImage.fillAmount = 0f;
    }
}
