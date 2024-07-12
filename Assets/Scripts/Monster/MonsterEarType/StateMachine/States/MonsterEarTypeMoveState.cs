using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeMoveState : MonsterEarTypeGroundState
{
    public MonsterEarTypeMoveState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.IsMove = true;
        stateMachine.MovementSpeedModifier = groundData.MoveSpeed;

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

        // 근처에 플레이어 있을시 공격
        if (IsInAttackRange())
        {
            Debug.Log("플레이어 사망 - 게임 오버  ");
            return;
        }

        if (Vector3.Distance(stateMachine.CurDestination, stateMachine.Monster.transform.position) < 1f)
        {
            // 없으면 집중 상태로 전환
            stateMachine.ChangeState(stateMachine.FocusState);
            return;
        }

        Move();
    }
}
