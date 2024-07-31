﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterEarTypePatrolState : MonsterEarTypeGroundState
{
    public MonsterEarTypePatrolState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        if (stateMachine.Monster.Agent.isStopped) stateMachine.Monster.Agent.isStopped = false;
        stateMachine.Monster.Agent.speed = groundData.PatrolSpeed;
        if (stateMachine.IsPatrol) return;

        StatrPatrol();

        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.PatrolParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //stateMachine.Monster.Agent.enabled = false;
        stateMachine.IsPatrol = false;
        // 애니메이션 종료
        StopAnimation(stateMachine.Monster.AnimationData.PatrolParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (stateMachine.Monster.Agent.pathPending) return;

        if (stateMachine.Monster.Agent.remainingDistance < 0.2f)
        {
            // 목적지에 도착하면 idle 상태 진입
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    private Vector3 GetRandomPoint(Vector3 center, float radius)
    {
        Vector3 randomPos = center;
        for (int i = 0; i < 50; i++)
        {
            randomPos = Random.insideUnitSphere * radius;
            randomPos.y = 0;
            randomPos += center;
            if (Vector3.Distance(stateMachine.Monster.transform.position, randomPos) > stateMachine.Monster.Data.GroundData.PatrolMinDistance) break;
        }

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas))
        {
            return hit.position;
        }
        return randomPos;
    }

    private void StatrPatrol()
    {
        Vector3 newPos = GetRandomPoint(stateMachine.StartPosition, groundData.PatrolRange);
        stateMachine.Monster.Agent.SetDestination(newPos);
        stateMachine.IsPatrol = true;
    }
}
