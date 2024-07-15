using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    private PlayerStamina CurrentStamina;
    private PlayerStamina CanRun;

    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        CurrentStamina = stateMachine.Player.GetComponent<PlayerStamina>();

        if (stateMachine.IsRuning)
        {
            stateMachine.ChangeState(stateMachine.RunState);
            return;
        }

        if (stateMachine.IsCrouch)
        {
            stateMachine.ChangeState(stateMachine.CrouchState);
            return;
        }

        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
    }

    public override void Update()
    {
        base.Update();
        CurrentStamina.IncreaseWalkIdle();
        if (CanRun == false && stateMachine.IsRuning)
        {
            stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.IsWalking = false; // 걷기 중단
    }

    protected override void OnRunPerformed(InputAction.CallbackContext context)
    {
        base.OnRunPerformed(context);

        if (CurrentStamina.CanRun)
        {
            stateMachine.ChangeState(stateMachine.RunState); // 스태미나가 충분할 때만 달리기 상태로 전환
        }
    }

    protected override void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        base.OnCrouchPerformed(context);
    }
}