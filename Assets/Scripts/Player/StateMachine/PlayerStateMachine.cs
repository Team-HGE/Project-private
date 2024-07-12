using UnityEngine.InputSystem.LowLevel;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    // 플레이어 상태
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    // 추가 상태들 추가 구현 사항***
    //public PlayerJumpState JumpState { get; }
    //public PlayerFallState FallState { get; }

    // 상태 전환 조건
    public bool IsRuning { get; set; }
    public bool IsCrouch { get; set; }
    // 상호작용
    public bool IsInteraction { get; set; }


    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float OriginHeight { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;
    public float JumpForce { get; set; }

    public PlayerStateMachine(Player player)
    {
        Player = player;
        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        CrouchState = new PlayerCrouchState(this);

        MovementSpeed = player.Data.GroundData.BaseSpeed;
        OriginHeight = player.transform.localScale.y;
    }
}