using UnityEditor.ShaderKeywordFilter;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRunState : PlayerGroundState
{
    private string stepTag = "RunStepNoise";
    private SoundSource curStepSource;
    private RunEffect CurrentStamina;

    public PlayerRunState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        CurrentStamina = stateMachine.Player.GetComponent<RunEffect>();

        if (CurrentStamina.IsExhausted)
        {
            stateMachine.ChangeState(stateMachine.WalkState);
        }
        else
        {
            stateMachine.MovementSpeedModifier = groundData.RunSpeedModifier;
           
        }
        stateMachine.Player.SumNoiseAmount = 12f;
    }

    public override void Update()
    {
        base.Update();
        CurrentStamina.ConsumeStamina(18f); // 스태미나 소모

        if (CurrentStamina.IsExhausted)
        {
            stateMachine.ChangeState(stateMachine.WalkState); // 스태미나가 다 떨어지면 걷기 상태로 전환
        }
        
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.IsRuning = false; 
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