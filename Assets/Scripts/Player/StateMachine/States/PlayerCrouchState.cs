using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static AudioManager;

public class PlayerCrouchState : PlayerGroundState
{
    float originHeight = 0f;
    public AudioManager audioManager;

    public PlayerCrouchState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("앉기");
        originHeight = stateMachine.Player.virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y;
        stateMachine.Player.virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = 3f;
        stateMachine.Player.transform.localScale = new Vector3(stateMachine.Player.transform.localScale.x, groundData.CrouchHeight, stateMachine.Player.transform.localScale.z);
        stateMachine.MovementSpeedModifier = groundData.CrouchSpeedModifier;
        stateMachine.Player.SumNoiseAmount = 2f;
        stateMachine.IsCrouch = true;
        AudioManager.Instance.PlaySoundEffect(SoundEffect.duck);
    }

    public override void Exit() 
    {
        base.Exit();
        AudioManager.Instance.PlaySoundEffect(SoundEffect.Wokeup);
        stateMachine.Player.transform.localScale = new Vector3(stateMachine.Player.transform.localScale.x, stateMachine.OriginHeight, stateMachine.Player.transform.localScale.z);
        stateMachine.Player.virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = originHeight;
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
            AudioManager.Instance.StopAllClips();
            return;
        }

        if (stateMachine.PressShift)
        {
            stateMachine.ChangeState(stateMachine.RunState);
            AudioManager.Instance.StopAllClips();
            return;
        }

        stateMachine.ChangeState(stateMachine.IdleState);
    }
}
