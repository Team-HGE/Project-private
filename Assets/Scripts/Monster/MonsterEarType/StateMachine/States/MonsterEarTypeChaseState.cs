using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeChaseState : MonsterEarTypeGroundState
{
    public MonsterEarTypeChaseState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("chace 시작");

        //stateMachine.Monster.Agent.isStopped = true;
        stateMachine.IsChasing = true;
        stateMachine.Monster.Agent.speed = groundData.ChaseSpeed;

        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("chace 끝");

        stateMachine.IsChasing = false;

        // 애니메이션 종료
        StopAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);

    }

    public override void Update()
    {
        base.Update();

        if (IsInAttackRange())
        {
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

        stateMachine.Monster.Agent.SetDestination(stateMachine.Target.transform.position);

    }
}
