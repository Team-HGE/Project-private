using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundState
{
    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override void OnRunCanceled(InputAction.CallbackContext context)
    {
        base.OnRunCanceled(context);

        if (stateMachine.Player.InputsData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
        else
        {
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }
    }
}