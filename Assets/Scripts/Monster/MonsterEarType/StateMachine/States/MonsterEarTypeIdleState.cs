using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeIdleState : MonsterEarTypeGroundState
{
    public MonsterEarTypeIdleState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 애니메이션 실행 - 그라운드 파라미터 해쉬로 접근
        //StartAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);//구현 예정***

        stateMachine.Monster.IsBehavior = false;
        stateMachine.Monster.WaitForBehavior(stateMachine.Monster.Data.GroundData.IdleTransitionTime);

    }

    public override void Exit()
    {
        base.Exit();

        // 애니메이션 종료 - 그라운드 파라미터 해쉬로 접근
        //StopAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);//구현 예정***
    }

    public override void Update()
    {
        base.Update();

        if (!stateMachine.Monster.IsBehavior) return;
        stateMachine.ChangeState(stateMachine.PatrolState);
    }
}
