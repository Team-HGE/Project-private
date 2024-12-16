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
        stateMachine.Player.SumNoiseAmount = 6f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        // E 키로 상호작용
        if (stateMachine.IsInteraction)
        {
            // 플레이어 정지 - 상호작용
            if (!stateMachine.Player.IsPlayerControll) return;

            //Debug.Log("Update - E키 누르는 중");
            GameManager.Instance.player.OnInteracted();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (!stateMachine.Player.Controller.isGrounded
        && stateMachine.Player.Controller.velocity.y < Physics.gravity.y * Time.fixedDeltaTime)
        {
            return;
        }
    }

    // E 키 누름
    protected override void OnInterationStared(InputAction.CallbackContext context)
    {
        base.OnInterationStared(context);

        //Debug.Log("E키 누름");
    }

    // E 키 누르는 중
    //protected override void OnInterationPerformed(InputAction.CallbackContext context)
    //{
    //    base.OnInterationPerformed(context);
    //
    //   
    //}

    // E 키 뗌
    protected override void OnInterationCanceled(InputAction.CallbackContext context)
    {
        base.OnInterationCanceled(context);

        //Debug.Log("E키 뗌");
        GameManager.Instance.player.EndInteraction();
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