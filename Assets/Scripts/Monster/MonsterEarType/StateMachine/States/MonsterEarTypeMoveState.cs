using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterEarTypeMoveState : MonsterEarTypeGroundState
{
    public MonsterEarTypeMoveState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.IsMove = true;
        stateMachine.Monster.Agent.speed = groundData.MoveSpeed;

        Debug.Log($"플레이어 추적 시작 - 무브, {stateMachine.CurDestination}");         
        // 작동안함
        stateMachine.Monster.Agent.SetDestination(stateMachine.CurDestination);

        // 애니메이션 실행

    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.IsMove = false;

        // 애니메이션 종료

    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Monster.Agent.remainingDistance < 1f)
        {
            Debug.Log("소음지역 도착");
            stateMachine.ChangeState(stateMachine.FocusState);
        }

    }

}
