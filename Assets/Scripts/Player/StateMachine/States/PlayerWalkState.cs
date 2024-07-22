using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    //private RunEffect CurrentStamina;

    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //CurrentStamina = stateMachine.Player.GetComponent<RunEffect>();

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

        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        stateMachine.Player.SumNoiseAmount = 6f;
    }

    public override void Update()
    {
        base.Update();
        stateMachine.Player.CurrentStamina.IncreaseWalkIdle();

        //if (CanRun == false && stateMachine.PressShift)
        //{
        //    stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        //}
        
    }

    public override void Exit()
    {
        base.Exit();
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