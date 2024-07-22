using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.ShaderKeywordFilter;


public class PlayerIdleState : PlayerGroundState
{
    //private RunEffect CurrentStamina;

    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.MovementSpeedModifier = 0f;

        //CurrentStamina = stateMachine.Player.GetComponent<RunEffect>();

        if (stateMachine.PressCtrl)
        {
            stateMachine.ChangeState(stateMachine.CrouchState);
            return;
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        stateMachine.Player.CurrentStamina.IncreaseStaminaIdle(); // Idle 상태에서 스태미나 회복

        if (stateMachine.Player.InputsData.MovementInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }

    protected override void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        base.OnCrouchPerformed(context);
    }

    protected override void OnRunPerformed(InputAction.CallbackContext context)
    {
        base.OnRunPerformed(context);

        if (stateMachine.Player.CurrentStamina.CanRun)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
    }
}