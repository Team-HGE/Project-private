using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerGroundState : PlayerBaseState
{
    public PlayerGroundState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        // 땅에 붙어있는 상태였다가 -> 추락 (걷가가 절벽으로 떨어진 경우)
        if (!stateMachine.Player.Controller.isGrounded
        && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            // 추락 이후에 소음이 발생 구현 예정 추가 구현 사항***
            return;
        }
    }

    protected override void OnMovementCanceled(InputAction.CallbackContext context)
    {
        base.OnMovementCanceled(context);

        if (stateMachine.Player.InputsData.MovementInput == Vector2.zero)
        {
            return;
        }

        stateMachine.ChangeState(stateMachine.IdleState);
    }

    protected override void OnRunPerformed(InputAction.CallbackContext context)
    {
        base.OnRunPerformed(context);
    }

    protected override void OnRunCanceled(InputAction.CallbackContext context)
    {
        base.OnRunCanceled(context);
    }


    protected override void OnJumpStarted(InputAction.CallbackContext context)
    {
        base.OnJumpStarted(context);

        // JumpState 로 전환 - 추가 구현 사항***
        //stateMachine.ChangeState(stateMachine.JumpState);
    }

    protected override void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        base.OnCrouchPerformed(context);

        stateMachine.ChangeState(stateMachine.CrouchState);
    }

    protected override void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        base.OnCrouchCanceled(context);
    }
}