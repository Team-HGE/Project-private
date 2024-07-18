using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class ElevatorBtn : InteractableObject
{
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material[] changeMaterial;
    [SerializeField] int materialIndexChange;
    [SerializeField] int myNum;
    [SerializeField] ElevatorObject elevatorObject;
    [SerializeField] UnityEvent<int> action;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        if (GameManager.Instance.isElevatorButtonPressed) return;
        if (GameManager.Instance.nowFloor == myNum) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "´©¸£±â";
    }
    public override void Interact()
    {
        isInteractable = true;
        GameManager.Instance.isElevatorButtonPressed = true;
        Material[] newMaterials = meshRenderer.materials;
        newMaterials[materialIndexChange] = changeMaterial[0];
        meshRenderer.materials = newMaterials;

        action?.Invoke(myNum);
        elevatorObject.onInteractComplete -= ChangeMaterialAfterAction;
        elevatorObject.onInteractComplete += ChangeMaterialAfterAction;
    }

    private void ChangeMaterialAfterAction()
    {
        Material[] newMaterials = meshRenderer.materials;
        newMaterials[materialIndexChange] = changeMaterial[1];
        meshRenderer.materials = newMaterials;
        elevatorObject.onInteractComplete -= ChangeMaterialAfterAction;
        isInteractable = false;
        GameManager.Instance.isElevatorButtonPressed = false;
    }
}
