using UnityEngine;

public class MonsterIdleState : MonsterGroundState
{
    public MonsterIdleState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //Debug.Log($"아이들 시작");

        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);

        stateMachine.Monster.IsBehavior = false;
        stateMachine.Monster.WaitForBehavior(stateMachine.Monster.Data.GroundData.IdleTransitionTime);
    }
    public override void Exit()
    {
        base.Exit();
        //Debug.Log($"아이들 끝");

        // 애니메이션 종료
        StopAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (!stateMachine.Monster.IsBehavior) return;
        stateMachine.ChangeState(stateMachine.PatrolState);
    }
}
