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

        // E 키를 누르는 동안 실행 시킬 내용
        if (stateMachine.IsInteraction)
        {
            Debug.Log("Update - E키 누르는 중");

        }
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

    // E 키 상호작용 - 키 눌렀을 때
    protected override void OnInterationStared(InputAction.CallbackContext context)
    {
        base.OnInterationStared(context);

        Debug.Log("E키 눌렸음");
    }

    // E 키 상호작용 - 키 누르는 중
    protected override void OnInterationPerformed(InputAction.CallbackContext context)
    {
        base.OnInterationPerformed(context);

        Debug.Log("E키 누르는 중");
    }

    // E 키 상호작용 - 키 뗌
    protected override void OnInterationCanceled(InputAction.CallbackContext context)
    {
        base.OnInterationCanceled(context);

        Debug.Log("E키 뗌");
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