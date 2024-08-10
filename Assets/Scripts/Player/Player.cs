using Cinemachine;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, INoise
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Noise")]
    [field: SerializeField] public NoiseDatasList NoiseDatasList { get; private set; }
    // INoise
    public float NoiseTransitionTime { get; set; }
    public float MaxNoiseAmount { get; set; } = 13f;
    [field: SerializeField] public float CurNoiseAmount { get; set; }
    public float SumNoiseAmount { get; set; }
    [field: SerializeField] public float DecreaseSpeed { get; set; } = 5f;

    [field: Header("Stamina")]
    public RunEffect CurrentStamina;

    [field: Header("VC")]
    public CinemachineVirtualCamera offSight;

    [field: Header("Controll")]
    // 플레이 조작 onoff
    [field: SerializeField] public bool IsPlayerControll { get; set; } = true;

    [field: Header("Controll")]
    public FlashLightController flashLightController;

    [field: Header("Karma")]
    [field: SerializeField] public float Karma { get; set; }

    public PlayerController Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public PlayerInputsData InputsData { get; private set; }

    private PlayerStateMachine _stateMachine;

    private void Awake()
    {
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        InputsData = GetComponent<PlayerInputsData>();
        CurrentStamina = GetComponent<RunEffect>();
        flashLightController = GetComponent<FlashLightController>();

        _stateMachine = new PlayerStateMachine(this);

        if (NoisePool.Instance != null)
        {
            //Debug.Log($"Player - Awake - 노이즈 풀 있음");
            NoisePool.Instance.Initialize();
        }

        for (int i = 0; i < NoiseDatasList.noiseDatasList.Count; i++)
        {
            NoisePool.Instance.noiseDatasList.Add(NoiseDatasList.noiseDatasList[i]);
        }

        NoisePool.Instance.FindNoise();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _stateMachine.ChangeState(_stateMachine.IdleState);
        // 카르마 초기화
        Karma = 0f;
        GameManager.Instance.Init(this);
    }

    private void Update()
    {
        _stateMachine.HandleInput();
        _stateMachine.Update();

        if (CurNoiseAmount > 0)
        {
            CurNoiseAmount -= DecreaseSpeed * Time.deltaTime;
            if (CurNoiseAmount <= 0) CurNoiseAmount = 0;
        }

        if (CurNoiseAmount >= MaxNoiseAmount) CurNoiseAmount = MaxNoiseAmount;
    }

    private void FixedUpdate()
    {
        _stateMachine.PhysicsUpdate();
    }

    public SoundSource PlayNoise(AudioClip[] audioClips, string tag, float amount, float addVolume, float transitionTime, float pitch)
    {
        int index = Random.Range(0, audioClips.Length);
        //Debug.Log(index);

        SoundSource soundSource;
        soundSource = NoiseManager.Instance.PlayNoise(audioClips[index], tag, addVolume, transitionTime, pitch);
        CurNoiseAmount += amount;
        if (CurNoiseAmount >= SumNoiseAmount) CurNoiseAmount = SumNoiseAmount;
        return soundSource;
    }

    public SoundSource PlayNoise(AudioClip audioClip, string tag, float amount, float addVolume, float transitionTime, float pitch)
    {                
        SoundSource soundSource;
        soundSource = NoiseManager.Instance.PlayNoise(audioClip, tag, addVolume, transitionTime, pitch);
        CurNoiseAmount += amount;
        if (CurNoiseAmount >= SumNoiseAmount) CurNoiseAmount = SumNoiseAmount;
        return soundSource;
    }

    public void PlayerControllOnOff()
    {
        //Debug.Log("Player - PlayerControllOnOff 호출됨");
        IsPlayerControll = !IsPlayerControll;

        if (IsPlayerControll) offSight.enabled = false;
        else offSight.enabled = true;
    }

    public void PlayerControllOff()
    {
        IsPlayerControll = false;
    }

    public void PlayerControllOn()
    {
        IsPlayerControll = true;
    }

    public Player GetPlayerReturn()
    {
        return this;
    }
    public FlashLightController GetFlashLightController()
    {
        return flashLightController;
    }
}