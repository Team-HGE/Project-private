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

        stateMachine.IsChasing = true;
        stateMachine.Monster.Agent.speed = groundData.ChaseSpeed;

        // 애니메이션 실행

    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.IsChasing = false;

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

        stateMachine.Monster.Agent.SetDestination(stateMachine.Target.transform.position);

    }
}
