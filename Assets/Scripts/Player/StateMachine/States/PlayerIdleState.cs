using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.ShaderKeywordFilter;


public class PlayerIdleState : PlayerGroundState
{
    private PlayerStamina CurrentStamina;

    public PlayerIdleState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.MovementSpeedModifier = 0f;

        CurrentStamina = stateMachine.Player.GetComponent<PlayerStamina>();

        if (stateMachine.IsCrouch)
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

        CurrentStamina.IncreaseStaminaIdle(); // Idle 상태에서 스태미나 회복

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

        if (CurrentStamina.CanRun)
        {
            stateMachine.ChangeState(stateMachine.RunState);
        }
    }
}