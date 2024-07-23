using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCrouchState : PlayerGroundState
{
    public PlayerCrouchState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("¾É±â");

        stateMachine.Player.transform.localScale = new Vector3(stateMachine.Player.transform.localScale.x, groundData.CrouchHeight, stateMachine.Player.transform.localScale.z);
        stateMachine.MovementSpeedModifier = groundData.CrouchSpeedModifier;
        stateMachine.Player.SumNoiseAmount = 2f;
        stateMachine.IsCrouch = true;
    }

    public override void Exit() 
    {
        base.Exit();
        stateMachine.Player.transform.localScale = new Vector3(stateMachine.Player.transform.localScale.x, stateMachine.OriginHeight, stateMachine.Player.transform.localScale.z);
        stateMachine.IsCrouch = false;
    }

    public override void Update()
    {
        base.Update();
    }

    protected override void OnCrouchCanceled(InputAction.CallbackContext context)
    {
        base.OnCrouchCanceled(context);

        if (stateMachine.Player.InputsData.MovementInput != Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }

        if (stateMachine.PressShift)
        {
            stateMachine.ChangeState(stateMachine.RunState);
            return;
        }

        stateMachine.ChangeState(stateMachine.IdleState);
    }
}
