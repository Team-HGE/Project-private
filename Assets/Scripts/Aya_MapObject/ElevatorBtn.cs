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

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }
    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        if (HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorButtonPressed) return;
        if (elevatorObject.NowFloor == myNum) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "´©¸£±â";
    }
    public override void Interact()
    {
        if (isInteractable) return;
        if (HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorButtonPressed) return;
        if (elevatorObject.NowFloor == myNum) return;
        isInteractable = true;
        HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorButtonPressed = true;

        Material[] newMaterials = meshRenderer.materials;
        newMaterials[materialIndexChange] = changeMaterial[0];
        meshRenderer.materials = newMaterials;
        elevatorObject.audioSource.PlayOneShot(elevatorObject.elevatorSounds[(int)ElevatorSoundType.BtnPressed].audioClip);
        elevatorObject.MoveFloor(myNum, true);
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
        HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorButtonPressed = false;
    }
}
