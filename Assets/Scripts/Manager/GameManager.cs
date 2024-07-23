using DiceNook.View;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonManager<GameManager>
{
    [Header("PlayerInteractionCanvas")]
    public GameObject playerInteractionCanvas;
    public TextMeshProUGUI interactableText;
    public Image interactionImage;
    public Image fillAmountImage;
    public PlayerInteractable player;

    [Header("Elevator")]
    private int _nowFloor = 1;
    public Transform blockCelling;
    public FadeManager fadeManager;
    public int nowFloor
    {
        get { return _nowFloor; }
        set { _nowFloor = value; }
    }
    public bool isElevatorButtonPressed;

    public ExampleOfUpdatingTheBar exampleBar;
    protected override void Awake()
    {
        base.Awake();
        if (fadeManager == null)
        {
            fadeManager = GetComponent<FadeManager>();
        }
    }
    public void Init(Player _player)
    {
        exampleBar.player = _player;
        player = _player.GetComponent<PlayerInteractable>();
        player.fillAmountImage = fillAmountImage;
        player.interactableText = interactableText;
        player.playerInteraction = playerInteractionCanvas;
        player.interactionImage = interactionImage;
    }
}
