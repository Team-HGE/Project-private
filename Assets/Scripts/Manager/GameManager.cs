using UnityEngine;

public class GameManager : SingletonManager<GameManager>
{
    public PlayerInteractable player;
    private int _nowFloor = 1;
    public Transform blockCelling;
    public int nowFloor
    {
        get { return _nowFloor; }
        set { _nowFloor = value; }
    }
    public bool isElevatorButtonPressed;

    protected override void Awake()
    {
        base.Awake();
    }
}
