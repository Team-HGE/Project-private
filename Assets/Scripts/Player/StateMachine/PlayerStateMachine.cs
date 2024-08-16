using UnityEngine.InputSystem.LowLevel;
using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }

    // 플레이어 상태
    public PlayerIdleState IdleState { get; private set; }
    public PlayerWalkState WalkState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerCrouchState CrouchState { get; private set; }

    // 상태 전환 조건
    public bool PressShift { get; set; }
    public bool PressCtrl { get; set; }

    // 현재 상태
    public bool IsRunning { get; set; }
    public bool IsCrouch { get; set; }
    public bool IsWalking { get; set; }

    // 상호작용
    public bool IsInteraction { get; set; }


    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float OriginHeight { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;

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

    public override void ChangeState(IState state)
    {
        if (!Player.IsPlayerControll) return;

        base.ChangeState(state);
    }
}