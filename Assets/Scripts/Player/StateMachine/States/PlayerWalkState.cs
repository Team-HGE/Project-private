using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerWalkState : PlayerGroundState
{
    private PlayerStamina CurrentStamina;
    private PlayerStamina CanRun;

    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        CurrentStamina = stateMachine.Player.GetComponent<PlayerStamina>();

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

    public override void Update()
    {
        base.Update();
        CurrentStamina.IncreaseWalkIdle();
        if (CanRun == false && stateMachine.IsRuning)
        {
            stateMachine.MovementSpeedModifier = groundData.WalkSpeedModifier;
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.IsWalking = false; // �ȱ� �ߴ�
    }

    protected override void OnRunPerformed(InputAction.CallbackContext context)
    {
        base.OnRunPerformed(context);

        if (CurrentStamina.CanRun)
        {
            stateMachine.ChangeState(stateMachine.RunState); // ���¹̳��� ����� ���� �޸��� ���·� ��ȯ
        }
    }

    protected override void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        base.OnCrouchPerformed(context);
    }
}