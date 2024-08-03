using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public bool isElevatorButtonPressed;
    public bool isElevatorOpen;
    public float openTime = 0;
    [SerializeField] ElevatorObject nowElevator;
    List<DOTweenAnimation> nowDoor = new List<DOTweenAnimation>();
    [SerializeField] DOTweenAnimation[] floorOpenAni;
    [SerializeField] DOTweenAnimation[] floorCloseAni;
    public List<DOTweenAnimation> GetOpenDoor(int nowFloor, ElevatorObject elevatorObject)
    {
        nowElevator = elevatorObject;
        nowDoor.Clear();
        switch (nowFloor)
        {
            case 1:
                if (nowElevator.elevatorIndex == 1)
                {
                    nowDoor.Add(floorOpenAni[0]);
                    nowDoor.Add(floorOpenAni[1]);
                }
                break;

        }
        return nowDoor;
    }
    public List<DOTweenAnimation> GetCloseDoor(int nowFloor, ElevatorObject elevatorObject)
    {
        nowElevator = elevatorObject;
        nowDoor.Clear();
        switch (nowFloor)
        {
            case 1:
                if (nowElevator.elevatorIndex == 1)
                {
                    nowDoor.Add(floorCloseAni[0]);
                    nowDoor.Add(floorCloseAni[1]);
                }
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
