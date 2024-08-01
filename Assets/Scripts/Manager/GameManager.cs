using DiceNook.View;
using System.Collections;
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

    [Header("Player")]
    public ExampleOfUpdatingTheBar exampleBar;
    public PlayerStateMachine PlayerStateMachine { get; set; }

    [Header("Manager")]
    public CinemachineManager cinemachineManager;
    public int nowFloor
    {
        get { return _nowFloor; }
        set { _nowFloor = value; }
    }
    public bool isElevatorButtonPressed;

    [Header("Time")]
    public DayNightUI dayNightUI;
    [Header("Light")]
    public LightManager lightManager;
    protected override void Awake()
    {
        base.Awake();
        if (fadeManager == null) fadeManager = GetComponent<FadeManager>();

        if (dayNightUI == null) dayNightUI = GetComponent<DayNightUI>();

        if (lightManager == null) lightManager = GetComponent<LightManager>();

        if (cinemachineManager == null) cinemachineManager = GetComponent<CinemachineManager>();
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
    private void Start()
    {
        StartCoroutine(ASceneLoading());
    }
    IEnumerator ASceneLoading()
    {
        yield return new WaitForSeconds(3);
        fadeManager.sceneLoadings[(int)SceneEnum.AScene].SetActive(false);
        fadeManager.FadeStart(FadeState.FadeIn);
        AudioManager.Instance.PlaySound(BackGroundSound.ASceneSound);
        DialogueManager.Instance.storyScript.Print();
    }
}
