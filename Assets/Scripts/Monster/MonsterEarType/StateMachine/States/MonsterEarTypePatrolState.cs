using System.Collections;
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
        //stateMachine.Monster.Agent.enabled = true;
        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.Monster.Agent.speed = groundData.PatrolSpeed;
        if (stateMachine.IsPatrol) return;

        StatrPatrol();

        // �ִϸ��̼� ���� - �׶��� �Ķ���� �ؽ��� ����
        StartAnimation(stateMachine.Monster.AnimationData.PatrolParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.Monster.Agent.isStopped = true;
        //stateMachine.Monster.Agent.enabled = false;
        stateMachine.IsPatrol = false;

        // �ִϸ��̼� ���� - �׶��� �Ķ���� �ؽ��� ����
        StopAnimation(stateMachine.Monster.AnimationData.PatrolParameterHash);

    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Monster.Agent.enabled && stateMachine.Monster.Agent.remainingDistance < 0.1f)
        {
            // �������� �����ϸ� idle ���� ����
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
