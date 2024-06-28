using TMPro;
using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    public PlayerInteractable player;

    protected override void Awake()
    {
        base.Awake();
    }
}