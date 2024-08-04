using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorUpDownBtn : InteractableObject
{
    [SerializeField] int floorIndex;
    [SerializeField] ElevatorObject elevatorObject;
    [SerializeField] MeshRenderer meshRenderer;
    [SerializeField] Material[] changeMaterial;
    [SerializeField] int materialIndexChange;
    [SerializeField] MeshRenderer lightDecoTop;
    [SerializeField] MeshRenderer lightDecoUnder;
    Material[] newLightDeco;
    bool isTopLight;

    public override void ActivateInteraction()
    {
        if (isInteractable) return;
        if (HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorButtonPressed) return;
        GameManager.Instance.player.playerInteraction.SetActive(true);
        GameManager.Instance.player.interactableText.text = "´©¸£±â";
    }

    public override void Interact()
    {
        if (isInteractable) return;
        if (HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorButtonPressed) return;
        HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorButtonPressed = true;
        if (elevatorObject.NowFloor != floorIndex)
        {
            if (elevatorObject.NowFloor > floorIndex)
            {
                newLightDeco = lightDecoUnder.materials;
                newLightDeco[0] = changeMaterial[0];
                lightDecoUnder.materials = newLightDeco;
                isTopLight = false;
            }
            else if (elevatorObject.NowFloor < floorIndex)
            {
                newLightDeco = lightDecoTop.materials;
                newLightDeco[0] = changeMaterial[0];
                lightDecoTop.materials = newLightDeco;
                isTopLight = true;
            }
            elevatorObject.MoveFloor(floorIndex, false);
        }
        else
        {
            elevatorObject.OpenDoor();
        }

        Material[] newMaterials = meshRenderer.materials;
        newMaterials[materialIndexChange] = changeMaterial[0];
        meshRenderer.materials = newMaterials;

        elevatorObject.onInteractComplete -= ChangeMaterialAfterAction;
        elevatorObject.onInteractComplete += ChangeMaterialAfterAction;
    }
    private void ChangeMaterialAfterAction()
    {
        Debug.Log("In");
        Material[] newMaterials = meshRenderer.materials;
        newMaterials[materialIndexChange] = changeMaterial[1];
        meshRenderer.materials = newMaterials;
        if (newLightDeco !=  null)
        {
            if (isTopLight)
            {
                newLightDeco[0] = changeMaterial[1];
                lightDecoTop.materials = newLightDeco;
            }
            else
            {
                newLightDeco[0] = changeMaterial[1];
                lightDecoUnder.materials = newLightDeco;
            }
            newLightDeco = null;
        }
        
        isInteractable = false;
        if (HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorButtonPressed)
        {
            HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorButtonPressed = false;
            Debug.Log(HotelFloorScene_DataManager.Instance.elevatorManager.isElevatorButtonPressed);
        }
        elevatorObject.onInteractComplete -= ChangeMaterialAfterAction;
    }
}
