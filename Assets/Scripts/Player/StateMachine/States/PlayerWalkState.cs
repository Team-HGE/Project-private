using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{

    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (stateMachine.PressShift && stateMachine.Player.CurrentStamina.CanRun && !stateMachine.Player.CurrentStamina.IsExhausted)
        {
            stateMachine.ChangeState(stateMachine.RunState);
            return;
        }

        if (stateMachine.PressCtrl)
        {
            stateMachine.ChangeState(stateMachine.CrouchState);
            return;
        }

        stateMachine.IsWalking = true;
        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Player.CurrentStamina.IncreaseWalkIdle();
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.IsWalking = false;
    }

    protected override void OnRunPerformed(InputAction.CallbackContext context)
    {
        base.OnRunPerformed(context);

        if (stateMachine.Player.CurrentStamina.CanRun && !stateMachine.Player.CurrentStamina.IsExhausted)
        {
            stateMachine.ChangeState(stateMachine.RunState); // 스태미나가 충분할 때만 달리기 상태로 전환
        }
    }

    protected override void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        base.OnCrouchPerformed(context);
    }
}