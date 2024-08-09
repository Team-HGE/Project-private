using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum ElevatorObjectType
{
    ALeft,
    ARight,
    BLeft,
    BRight
}
[Serializable]
public class DOTweenAnimationListALeft
{
    [Header("Open")]
    public DOTweenAnimation floorLeftDoorOpen;
    public DOTweenAnimation floorRightDoorOpen;
    [Header("Close")]
    public DOTweenAnimation floorLeftDoorClose;
    public DOTweenAnimation floorRightDoorClose;
}
[Serializable]
public class DOTweenAnimationListARight
{
    [Header("Open")]
    public DOTweenAnimation floorLeftDoorOpen;
    public DOTweenAnimation floorRightDoorOpen;
    [Header("Close")]
    public DOTweenAnimation floorLeftDoorClose;
    public DOTweenAnimation floorRightDoorClose;
}
[Serializable]
public class DOTweenAnimationListBLeft
{
    [Header("Open")]
    public DOTweenAnimation floorLeftDoorOpen;
    public DOTweenAnimation floorRightDoorOpen;
    [Header("Close")]
    public DOTweenAnimation floorLeftDoorClose;
    public DOTweenAnimation floorRightDoorClose;
}
[Serializable]
public class DOTweenAnimationListBRight
{
    [Header("Open")]
    public DOTweenAnimation floorLeftDoorOpen;
    public DOTweenAnimation floorRightDoorOpen;
    [Header("Close")]
    public DOTweenAnimation floorLeftDoorClose;
    public DOTweenAnimation floorRightDoorClose;
}

public class ElevatorManager : MonoBehaviour
{
    [Header("PlayerHeightY")]
    public float playerHeightY;

    [Header("Bool Check")]
    public bool isElevatorButtonPressed;
    public bool isElevatorOpen;
    public float openTime = 0;

    List<DOTweenAnimation> nowDoor = new List<DOTweenAnimation>();

    [Header("FloorElevatorDoorAnimation")]
    [SerializeField] ElevatorObject nowElevator;
    [SerializeField] DOTweenAnimationListALeft[] aLeftFloorElevatorDoorAni;
    [SerializeField] DOTweenAnimationListARight[] aRightFloorElevatorDoorAni;
    [SerializeField] DOTweenAnimationListBLeft[] bLeftFloorElevatorDoorAni;
    [SerializeField] DOTweenAnimationListBRight[] bRightFloorElevatorDoorAni;
    public List<DOTweenAnimation> GetOpenDoor(int nowFloor, ElevatorObject elevatorObject)
    {
        nowElevator = elevatorObject;
        nowDoor.Clear();
        switch (nowElevator.elevatorObjectType)
        {
            case ElevatorObjectType.ALeft:
                nowDoor.Add(aLeftFloorElevatorDoorAni[nowElevator.NowFloor].floorLeftDoorOpen);
                nowDoor.Add(aLeftFloorElevatorDoorAni[nowElevator.NowFloor].floorRightDoorOpen);
                break;
            case ElevatorObjectType.ARight:
                nowDoor.Add(aRightFloorElevatorDoorAni[nowElevator.NowFloor].floorLeftDoorOpen);
                nowDoor.Add(aRightFloorElevatorDoorAni[nowElevator.NowFloor].floorRightDoorOpen);
                break;
            case ElevatorObjectType.BLeft:
                nowDoor.Add(bLeftFloorElevatorDoorAni[nowElevator.NowFloor].floorLeftDoorOpen);
                nowDoor.Add(bLeftFloorElevatorDoorAni[nowElevator.NowFloor].floorRightDoorOpen);
                break;
            case ElevatorObjectType.BRight:
                nowDoor.Add(bRightFloorElevatorDoorAni[nowElevator.NowFloor].floorLeftDoorOpen);
                nowDoor.Add(bRightFloorElevatorDoorAni[nowElevator.NowFloor].floorRightDoorOpen);
                break;
        }
        return nowDoor;
    }
    public List<DOTweenAnimation> GetCloseDoor(int nowFloor, ElevatorObject elevatorObject)
    {
        nowElevator = elevatorObject;
        nowDoor.Clear();
        switch (nowElevator.elevatorObjectType)
        {
            case ElevatorObjectType.ALeft:
                nowDoor.Add(aLeftFloorElevatorDoorAni[nowElevator.NowFloor].floorLeftDoorClose);
                nowDoor.Add(aLeftFloorElevatorDoorAni[nowElevator.NowFloor].floorRightDoorClose);
                break;
            case ElevatorObjectType.ARight:
                nowDoor.Add(aRightFloorElevatorDoorAni[nowElevator.NowFloor].floorLeftDoorClose);
                nowDoor.Add(aRightFloorElevatorDoorAni[nowElevator.NowFloor].floorRightDoorClose);
                break;
            case ElevatorObjectType.BLeft:
                nowDoor.Add(bLeftFloorElevatorDoorAni[nowElevator.NowFloor].floorLeftDoorClose);
                nowDoor.Add(bLeftFloorElevatorDoorAni[nowElevator.NowFloor].floorRightDoorClose);
                break;
            case ElevatorObjectType.BRight:
                nowDoor.Add(bRightFloorElevatorDoorAni[nowElevator.NowFloor].floorLeftDoorClose);
                nowDoor.Add(bRightFloorElevatorDoorAni[nowElevator.NowFloor].floorRightDoorClose);
                break;
        }
        return nowDoor;
    }

    private void Update()
    {
        if (isElevatorOpen && !isElevatorButtonPressed)
        {
            openTime += Time.deltaTime;

            if (openTime > 10)
            {
                nowElevator.CloseDoor();
                isElevatorOpen = false;
                openTime = 0;
            }
        }
    }
}
