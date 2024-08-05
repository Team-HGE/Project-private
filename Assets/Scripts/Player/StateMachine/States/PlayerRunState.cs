using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundState
{
    //private RunEffect CurrentStamina;

    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();


        //CurrentStamina = stateMachine.Player.GetComponent<RunEffect>();

        if (stateMachine.Player.CurrentStamina.IsExhausted)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
        else
        {
            stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
           
        }

        //Debug.Log("달리기 시작");
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        stateMachine.Player.SumNoiseAmount = 12f;
        stateMachine.IsRunning = true;
    }


    public override void Update()
    {
        base.Update();
        stateMachine.Player.CurrentStamina.ConsumeStamina(3f); // 스태미나 소모

        if (stateMachine.Player.CurrentStamina.IsExhausted)
        {
            stateMachine.ChangeState(stateMachine.WalkState); // 스태미나가 다 떨어지면 걷기 상태로 전환
        }
        
    }

    public override void Exit()
    {
        base.Exit();        
        stateMachine.IsRunning = false;
    }

    protected override void OnRunCanceled(InputAction.CallbackContext context)
    {
        base.OnRunCanceled(context);

        if (stateMachine.Player.InputsData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        else
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
    }

    protected override void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        base.OnCrouchPerformed(context);
    }

}