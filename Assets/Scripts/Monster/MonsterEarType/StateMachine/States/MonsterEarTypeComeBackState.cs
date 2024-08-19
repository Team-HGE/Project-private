    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeComeBackState : MonsterEarTypeGroundState
{
    public MonsterEarTypeComeBackState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!stateMachine.Monster.CanComeBack)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        //Debug.Log("컴백 시작");
        stateMachine.IsComeBack = true;
        stateMachine.Monster.Agent.speed = groundData.ComebackSpeed;
        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.ComeBackParameterHash);
        stateMachine.Monster.Agent.SetDestination(stateMachine.StartPosition);
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log("컴백 끝");
        stateMachine.IsComeBack = false;
        // 애니메이션 종료 - 그라운드 파라미터 해쉬로 접근
        StopAnimation(stateMachine.Monster.AnimationData.ComeBackParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Monster.Agent.pathPending) return;

        if (stateMachine.Monster.Agent.remainingDistance < 0.2f)
        {
            // 목적지에 도착
            //Debug.Log("복귀 완료");
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }
    }
}
