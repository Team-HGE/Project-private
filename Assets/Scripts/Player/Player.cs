using UnityEngine;

public class Player : MonoBehaviour
{
    [field: Header("References")]
    [field: SerializeField] public PlayerSO Data { get; private set; }

    public PlayerController Input { get; private set; }
    public CharacterController Controller { get; private set; }
    public ForceReceiver ForceReceiver { get; private set; }
    public PlayerInputsData InputsData { get; private set; }

    private PlayerStateMachine stateMachine;

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
}