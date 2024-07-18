using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundState
{
    private string stepTag = "RunStepNoise";
    private SoundSource curStepSource;

    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("달리기 시작");
        stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
        stateMachine.Player.SumNoiseAmount = 12f;
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("달리기 종료");

    }

    public override void Update()
    {
        base.Update();
        //Run();
    }

    protected override void OnRunCanceled(InputAction.CallbackContext context)
    {
        base.OnRunCanceled(context);

        if (stateMachine.Player.InputsData.MovementInput == Vector2.zero)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
        else
        {
            stateMachine.ChangeState(stateMachine.WalkState);
            return;
        }
    }

    protected override void OnCrouchPerformed(InputAction.CallbackContext context)
    {
        base.OnCrouchPerformed(context);
    }

    //private void Run()
    //{
    //    NoiseData curStepData;

    //    if (curStepSource == null)
    //    {
    //        for (int i = 0; i < stateMachine.Player.NoiseDatasList.noiseDatasList.Count; i++)
    //        {
    //            if (stateMachine.Player.NoiseDatasList.noiseDatasList[i].tag == stepTag)
    //            {
    //                curStepData = stateMachine.Player.NoiseDatasList.noiseDatasList[i];
    //                curStepSource = stateMachine.Player.PlayNoise(curStepData.noises, curStepData.tag, curStepData.volume, 0.2f, curStepData.transitionTime, 0f);
    //                break;
    //            }
    //        }
    //    }
    //    else
    //    {
    //        if (!curStepSource.gameObject.activeSelf)
    //        {
    //            curStepSource = null;
    //        }
    //    }
    //}
}