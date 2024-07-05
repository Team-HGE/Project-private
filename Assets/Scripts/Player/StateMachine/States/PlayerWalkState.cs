using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    bool isNoise = false;
    float delay = 0f;
    float delayMax = 30f;


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
        //SoundManager.Instance.PlayNoise(stateMachine.Player.NoiseDatas.noiseDatas[0].noises[0], "PlayerWalk");//임시코드 수정할것***




    }

    public override void Exit()
    {
        base.Exit();
    }


    public override void Update()
    {
        base.Update();
        delay += 1 * Time.deltaTime;

        if (delay <= delayMax)
        {
            delay = 0f;
        }

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