using System;
using System.Collections;
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
    public float holdTime = 2.0f; // 상호작용할 시간

    [Header("SerializeField")]
    [SerializeField] float interactionRange = 2.0f; // 사거리
    [SerializeField] float holdDuration = 0f; // 현재상호작용 시간
    [SerializeField] LayerMask layerMask;
    [SerializeField] Camera camera;
    
    IInteractable curInteractable;
    float checkRate = 1.0f;
    float lastCheckTime;

    public bool tutorialSuccess = false;

    private void Start()
    {
        camera = GetComponent<Camera>();
        camera = Camera.main;

    }
    void Update()
    {
        //if (Time.time - lastCheckTime > checkRate)
        //{
        //    lastCheckTime = Time.time;
        //    InteractWithObject();
        //}
        InteractWithObject();
    }

    void InteractWithObject()
    {
        Ray ray = camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionRange, layerMask))
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
            playerInteraction.SetActive(false);
            holdDuration = 0f;
            fillAmountImage.fillAmount = 0f;
        }
    }

    public void OnInteracted()
    {
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
                    if (!tutorialSuccess) tutorialSuccess = true;
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
