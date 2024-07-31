﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeIdleState : MonsterEarTypeGroundState
{
    public MonsterEarTypeIdleState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);
        stateMachine.Monster.Agent.isStopped = true;
        stateMachine.Monster.IsBehavior = false;
        stateMachine.Monster.WaitForBehavior(stateMachine.Monster.Data.GroundData.IdleTransitionTime);
    }

    public override void Exit()
    {
        base.Exit();

        // 애니메이션 종료
        StopAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);
        stateMachine.Monster.Agent.isStopped = false;
    }

    public override void Update()
    {
        base.Update();
        if (!stateMachine.Monster.CanPatrol) return;
        if (!stateMachine.Monster.IsBehavior) return;
        stateMachine.ChangeState(stateMachine.PatrolState);
    }
}
