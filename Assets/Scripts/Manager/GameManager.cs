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

    [Header("Player")]
    public ExampleOfUpdatingTheBar exampleBar;
    public PlayerStateMachine PlayerStateMachine { get; set; }

    [Header("Manager")]
    public FadeManager fadeManager;
    public CinemachineManager cinemachineManager;
    public LightManager lightManager;

    [Header("Time")]
    public DayNightUI dayNightUI;
    
    protected override void Awake()
    {
        base.Awake();
        if (fadeManager == null) fadeManager = GetComponent<FadeManager>();

        if (dayNightUI == null) dayNightUI = GetComponent<DayNightUI>();

        if (cinemachineManager == null) cinemachineManager = GetComponent<CinemachineManager>();

        if (lightManager == null) lightManager = GetComponent<LightManager>();
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
