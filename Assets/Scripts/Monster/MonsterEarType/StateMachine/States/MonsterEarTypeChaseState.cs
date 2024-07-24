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
        //Debug.Log("chace ����");

        stateMachine.IsChasing = true;
        stateMachine.Monster.Agent.speed = groundData.ChaseSpeed;

        // �ִϸ��̼� ����
        StartAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.IsChasing = false;

        // �ִϸ��̼� ����
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
