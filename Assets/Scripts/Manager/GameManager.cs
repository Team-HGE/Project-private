using DiceNook.View;
using Sirenix.OdinInspector;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : SingletonManager<GameManager>
{

    [TitleGroup("GameManager", "Singleton", alignment: TitleAlignments.Centered, horizontalLine: true, boldTitle: true, indent: false)]
    [InfoBox("인스펙터 꾸미는 기능 필요한게 있으시면 말씀해주시면 됩니다.")]
    [SerializeField] GameObject 응애;

    [TabGroup("Tab", "PlayerCanvas", SdfIconType.Image, TextColor = "lightgreen")]
    [TabGroup("Tab", "PlayerCanvas")] public GameObject playerInteractionCanvas;
    [TabGroup("Tab", "PlayerCanvas")] public TextMeshProUGUI interactableText;
    [TabGroup("Tab", "PlayerCanvas")] public Image interactionImage;
    [TabGroup("Tab", "PlayerCanvas")] public Image fillAmountImage;
    [TabGroup("Tab", "PlayerCanvas")] public PlayerInteractable player;

    [TabGroup("Tab", "Manager", SdfIconType.GearFill, TextColor = "orange")]
    [TabGroup("Tab", "Manager")] public FadeManager fadeManager;
    [TabGroup("Tab", "Manager")] public CinemachineManager cinemachineManager;
    [TabGroup("Tab", "Manager")] public LightManager lightManager;
    [TabGroup("Tab", "Manager")] public JumpScareManager jumpScareManager;

    [TitleGroup("Time")]
    public DayNightUI dayNightUI;

    [TitleGroup("Player")]
    public ExampleOfUpdatingTheBar exampleBar;
    [ShowInInspector] public PlayerStateMachine PlayerStateMachine { get; set; }

    protected override void Awake()
    {
        base.Awake();
        if (fadeManager == null) fadeManager = GetComponent<FadeManager>();

        if (dayNightUI == null) dayNightUI = GetComponent<DayNightUI>();

        if (cinemachineManager == null) cinemachineManager = GetComponent<CinemachineManager>();

        if (lightManager == null) lightManager = GetComponent<LightManager>();

        if (jumpScareManager == null) jumpScareManager = GetComponent<JumpScareManager>();
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
        
        fadeManager.FadeStart(FadeState.FadeIn);
        AudioManager.Instance.PlaySound(BackGroundSound.ASceneSound);
        DialogueManager.Instance.StartStory(1);
    }
}
