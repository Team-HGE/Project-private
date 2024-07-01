using UnityEngine;

public class PlayerIdleState : PlayerGroundState
{
    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.MovementSpeedModifier = 0f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Player.InputsData.MovementInput != Vector2.zero)
        {
            if (stateMachine.IsRuning)
            {
                stateMachine.ChangeState(stateMachine.RunState);
                return;
            }
            else 
            {
                stateMachine.ChangeState(stateMachine.WalkState);
                return;
            }            
        }
    }
}