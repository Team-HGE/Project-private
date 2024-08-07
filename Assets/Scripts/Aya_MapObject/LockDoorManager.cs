using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockDoorManager : MonoBehaviour
{
    [SerializeField] List<DoorObject> doorObjects = new List<DoorObject>();

    public void LockDoor()
    {
        foreach (DoorObject obj in doorObjects)
        {

        }
    }
}
