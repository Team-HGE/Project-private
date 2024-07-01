public class PlayerStateMachine : StateMachine
{

    public Player Player { get; }

    // �÷��̾� ����
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }

    // �߰� ���� ���� ����**
    //public PlayerJumpState JumpState { get; }

    // ���� ��ȯ�� ���� ����
    public bool IsRuning { get; set; }


    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;
    public float JumpForce { get; set; }

    public PlayerStateMachine(Player player)
    {
        Player = player;
        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        // �߰� ���µ� ���� ����**
        //JumpState = new PlayerJumpState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
    }
}