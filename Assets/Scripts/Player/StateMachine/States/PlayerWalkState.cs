using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {        
        base.Enter();

        if (stateMachine.IsRuning)
        {
            stateMachine.ChangeState(stateMachine.RunState);
            return;
        }

        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;       
    }

    public override void Exit()
    {
        base.Exit();
    }

    protected override void OnRunPerformed(InputAction.CallbackContext context)
    {
        base.OnRunPerformed(context);

        stateMachine.ChangeState(stateMachine.RunState);
    }
}