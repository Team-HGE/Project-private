using UnityEngine.InputSystem;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class PlayerWalkState : PlayerGroundState
{
    public PlayerWalkState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    bool isNoise = false;
    float delay = 0f;
    float delayMax = 30f;

    private SoundSource curAudioSource;

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

        //stateMachine.Player.PlayNoise(stateMachine.Player.NoiseDatas.noiseDatas[0].noises[0], "PlayerWalk");
        //Debug.Log(stateMachine.Player.NoiseDatas.noiseDatas[0].noises.Length);
        //stateMachine.Player.PlayNoise(stateMachine.Player.NoiseDatas.noiseDatas[0].noises, "PlayerWalk");        
    }

    public override void Exit()
    {
        base.Exit();
    }


    public override void Update()
    {
        base.Update();

        if (curAudioSource == null)
        {
            curAudioSource = stateMachine.Player.PlayNoise(stateMachine.Player.NoiseDatas.noiseDatas[0].noises, "PlayerWalk");

        }
        else 
        {
            if (!curAudioSource.gameObject.activeSelf)
            {
                curAudioSource = null;
            }

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