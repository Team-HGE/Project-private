using UnityEngine;

public class Player : MonoBehaviour, INoise
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    [field: Header("Noise")]
    [field: SerializeField] public NoiseData[] NoiseDatas { get; private set; }

    public PlayerController Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public PlayerInputsData InputsData { get; private set; }

    private PlayerStateMachine stateMachine;

    // INoise
    public float NoiseTransitionTime { get; set; }
    public float NoiseMin { get; set; }
    public float NoiseMax { get; set; }
    public float NoiseAmount { get; set; }
    public float DecreaseSpeed { get; set; }

    private void Awake()
    {
        Input = GetComponent<PlayerController>();
        Controller = GetComponent<CharacterController>();
        ForceReceiver = GetComponent<ForceReceiver>();
        InputsData = GetComponent<PlayerInputsData>();

        stateMachine = new PlayerStateMachine(this);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        stateMachine.ChangeState(stateMachine.IdleState);
    }

    private void Update()
    {
        stateMachine.HandleInput();
        stateMachine.Update();
    }

    private void FixedUpdate()
    {
        stateMachine.PhysicsUpdate();
    }

    public void MakeNoise(float transitionTime, float min, float max, float speed)
    {
        // 소음 반복
        if (transitionTime > 0)
        {

        }
        // 소음 한번 발생
        else
        {

        }
    }

    public void PlayNoise()
    {
        throw new System.NotImplementedException();
    }
}