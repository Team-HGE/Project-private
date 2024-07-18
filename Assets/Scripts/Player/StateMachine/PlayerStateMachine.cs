using UnityEngine.InputSystem.LowLevel;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    // �÷��̾� ����
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }
    // �߰� ���µ� �߰� ���� ����***
    //public PlayerJumpState JumpState { get; }
    //public PlayerFallState FallState { get; }

    // ���� ��ȯ ����
    public bool IsRuning { get; set; }
    public bool IsWalking { get; set; }
    public bool IsCrouch { get; set; }

    // ��ȣ�ۿ�
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