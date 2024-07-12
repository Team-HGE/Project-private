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

        stateMachine.IsChasing = true;
        stateMachine.MovementSpeedModifier = groundData.ChaseSpeed;

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
            Debug.Log("플레이어 사망 - 게임 오버  ");
            return;
        }

        Move();
    }
}
