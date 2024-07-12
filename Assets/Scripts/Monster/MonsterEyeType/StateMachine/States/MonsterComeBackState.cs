using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterComeBackState : MonsterGroundState
{
    public MonsterComeBackState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.Monster.Agent.speed = groundData.PatrolSpeed;
        stateMachine.Monster.Agent.SetDestination(stateMachine.StartPosition);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (IsInChaseRange() && GetIsPlayerInFieldOfView())
        {
            stateMachine.ChangeState(stateMachine.FindState);
            return;
        }

        if (stateMachine.Monster.Agent.remainingDistance < 0.1f)
        {
            stateMachine.ChangeState(stateMachine.PatrolState);
        }
    }
}
