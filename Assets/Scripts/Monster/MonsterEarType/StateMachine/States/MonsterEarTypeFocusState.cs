using UnityEngine;
public class MonsterEarTypeFocusState : MonsterEarTypeGroundState
{
    public MonsterEarTypeFocusState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //Debug.Log("focus 시작");
        stateMachine.BeforeNoise = 0f;
        stateMachine.IsFocusNoise = true;
        stateMachine.IsFocusRotate = true;
        stateMachine.Monster.IsBehavior = false;
        stateMachine.Monster.Agent.isStopped = true;
        stateMachine.Monster.WaitForBehavior(stateMachine.Monster.Data.GroundData.FocusTransitionTime);

        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.FocusParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("focus 끝");
        stateMachine.IsFocusNoise = false;
        stateMachine.IsFocusRotate = false;
        stateMachine.Monster.Agent.isStopped = false;

        // 애니메이션 종료
        StopAnimation(stateMachine.Monster.AnimationData.FocusParameterHash);
        stateMachine.Monster.StopWait();
        //stateMachine.Monster.IsBehavior = true;
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Monster.IsBehavior)
        {
            if (!stateMachine.Monster.CanComeBack)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }

            //Debug.Log("집중 -> 복귀");

            stateMachine.ChangeState(stateMachine.ComeBackState);
            return;
        }
    }
}
