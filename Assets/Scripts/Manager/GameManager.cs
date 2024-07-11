using System;
using TMPro;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    public PlayerInteractable player;
    public int nowFloor = 1;
    public bool isElevatorButtonPressed;

    protected override void Awake()
    {
        base.Awake();
    }
}
