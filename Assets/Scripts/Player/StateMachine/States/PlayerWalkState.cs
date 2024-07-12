using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System.Collections.Generic;
using System;

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

        if (stateMachine.IsCrouch)
        {
            stateMachine.ChangeState(stateMachine.CrouchState);
            return;
        }

        stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;    
    }

    public override void Exit()
    {
        base.Exit();
    }


    public override void Update()
    {
        base.Update();
    }


    protected override void OnRunPerformed(InputAction.CallbackContext context)
    {
        base.OnRunPerformed(context);

        stateMachine.ChangeState(stateMachine.RunState);
    }

    protected override void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        base.OnCrouchPerformed(context);
    }
}