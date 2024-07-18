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

        if (stateMachine.IsChasing || stateMachine.IsFocusNoise) return;

        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.IsMove = true;
        stateMachine.Monster.Agent.speed = groundData.MoveSpeed;

        //Debug.Log($"소음 추적 시작 - 무브, {stateMachine.CurDestination}");         

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

        if (IsInAttackRange())
        {
            Debug.Log("플레이어 사망 - 게임 오버");

            //stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        if (stateMachine.Monster.Agent.remainingDistance < 1f)
        {
            Debug.Log("소음지역 도착");
            stateMachine.ChangeState(stateMachine.FocusState);
            return;
        }

    }

}
