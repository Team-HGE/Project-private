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
        Debug.Log("chace ����");

        stateMachine.IsChasing = true;
        stateMachine.Monster.Agent.speed = groundData.ChaseSpeed;

        // �ִϸ��̼� ����

    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.IsChasing = false;

        // �ִϸ��̼� ����

    }

    public override void Update()
    {
        base.Update();

        if (IsInAttackRange())
        {
            Debug.Log("�÷��̾� ��� - ���� ����");

            //stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        stateMachine.Monster.Agent.SetDestination(stateMachine.Target.transform.position);

    }
}
