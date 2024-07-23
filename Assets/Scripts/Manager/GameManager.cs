using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonManager<GameManager>
{
    public PlayerInteractable player;
    private int _nowFloor = 1;
    public Transform blockCelling;
    public FadeManager fadeManager;
    public int nowFloor
    {
        get { return _nowFloor; }
        set { _nowFloor = value; }
    }
    public bool isElevatorButtonPressed;

    protected override void Awake()
    {
        base.Awake();
        if (fadeManager == null)
        {
            fadeManager = GetComponent<FadeManager>();
        }
    }
}
