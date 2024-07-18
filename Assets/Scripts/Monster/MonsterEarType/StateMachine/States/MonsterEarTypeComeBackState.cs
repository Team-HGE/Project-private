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
        stateMachine.IsComeBack = true;
        stateMachine.Monster.Agent.speed = groundData.ComebackSpeed;
        stateMachine.Monster.Agent.SetDestination(stateMachine.StartPosition);

        // 애니메이션 실행 - 그라운드 파라미터 해쉬로 접근

    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.IsComeBack = false;

        // 애니메이션 종료 - 그라운드 파라미터 해쉬로 접근

    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Monster.Agent.remainingDistance < 1f)
        {
            Debug.Log("복귀 완료");
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
