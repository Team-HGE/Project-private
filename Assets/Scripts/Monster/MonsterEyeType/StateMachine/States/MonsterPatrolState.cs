using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPatrolState : MonsterGroundState
{
    public MonsterPatrolState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.Monster.Agent.speed = groundData.PatrolSpeed;
        if (stateMachine.IsPatrol) return;

        StatrPatrol();

        //Debug.Log($"패트롤 시작");
        // 애니메이션 실행 - 그라운드 파라미터 해쉬로 접근
        StartAnimation(stateMachine.Monster.AnimationData.PatrolParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log($"패트롤 끝");

        // 애니메이션 종료 - 그라운드 파라미터 해쉬로 접근
        StopAnimation(stateMachine.Monster.AnimationData.PatrolParameterHash);
        stateMachine.IsPatrol = false;
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Monster.Agent.remainingDistance < 0.1f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }

        //if (IsInFindRange() && GetIsPlayerInFieldOfView())
        //{
        //    stateMachine.ChangeState(stateMachine.FindState);
        //}
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
