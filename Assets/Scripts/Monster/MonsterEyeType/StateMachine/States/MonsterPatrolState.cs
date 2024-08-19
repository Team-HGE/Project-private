using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterPatrolState : MonsterGroundState
{
    private Vector3 randomPos;
    //private bool getPosition;

    public MonsterPatrolState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.Monster.Agent.speed = groundData.PatrolSpeed;
        //if (stateMachine.IsPatrol) return;

        StatrPatrol();

        ///Debug.Log($"패트롤 시작");
        // 애니메이션 실행
        //StartAnimation(stateMachine.Monster.AnimationData.PatrolParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log($"패트롤 끝");

        // 애니메이션 종료
        if(stateMachine.IsPatrol) StopAnimation(stateMachine.Monster.AnimationData.PatrolParameterHash);

        //StopAnimation(stateMachine.Monster.AnimationData.PatrolParameterHash);
        
        stateMachine.Monster.Agent.ResetPath();
        stateMachine.IsPatrol = false;
    }

    public override void Update()
    {
        base.Update();

        //RotateToPlayer();

        if (stateMachine.Monster.Agent.pathPending) return;

        if (stateMachine.Monster.Agent.remainingDistance < 0.2f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }

    private void GetRandomPoint(Vector3 center, float radius)
    {
        //getPosition = true;
        randomPos = center;

        for (int i = 0; i < 50; i++)
        {
            randomPos = Random.insideUnitSphere * radius;
            randomPos.y = 0;
            randomPos += center;

            if (Vector3.Distance(center, randomPos) > stateMachine.Monster.patrolRangeMin) break;
        }

        //Debug.Log($"기준 이동 거리 : {Vector3.Distance(center, randomPos)}");
        //Debug.Log($"총 이동 거리 : {Vector3.Distance(stateMachine.Monster.transform.position, randomPos)}");

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPos, out hit, radius, NavMesh.AllAreas))
        {
            randomPos =  hit.position;
        }
    }

    private void StatrPatrol()
    {
        if (stateMachine.Monster.CanComeBack)
        {
            GetRandomPoint(stateMachine.StartPosition, stateMachine.Monster.patrolRangeMax);

            stateMachine.Monster.Agent.SetDestination(randomPos);
        }
        else
        {
            GetRandomPoint(stateMachine.Monster.transform.position, stateMachine.Monster.patrolRangeMax);

            stateMachine.Monster.Agent.SetDestination(randomPos);
        }

        StartAnimation(stateMachine.Monster.AnimationData.PatrolParameterHash);
        stateMachine.IsPatrol = true;
    }
}
