using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeComeBackState : MonsterEarTypeGroundState
{
    public MonsterEarTypeComeBackState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.IsComeBack = true;
        // 애니메이션 실행 - 그라운드 파라미터 해쉬로 접근

    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.IsComeBack = false;

        // 애니메이션 종료 - 그라운드 파라미터 해쉬로 접근

    }

    public override void Update()
    {
        base.Update();

        if (Vector3.Distance(stateMachine.StartPosition, stateMachine.Monster.transform.position) < 1f)
        {
            stateMachine.ChangeState(stateMachine.PatrolState);
            return;
        }

        Move();
    }
}
