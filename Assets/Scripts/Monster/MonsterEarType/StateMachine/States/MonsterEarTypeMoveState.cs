using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterEarTypeMoveState : MonsterEarTypeGroundState
{
    public MonsterEarTypeMoveState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (stateMachine.IsChasing || stateMachine.IsFocusNoise) return;

        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.IsMove = true;
        stateMachine.Monster.Agent.speed = groundData.MoveSpeed;

        //Debug.Log($"���� ���� ���� - ����, {stateMachine.CurDestination}");         

        stateMachine.Monster.Agent.SetDestination(stateMachine.CurDestination);

        // �ִϸ��̼� ����

    }

    public override void Exit()
    {
        base.Exit();

        stateMachine.IsMove = false;

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

        if (stateMachine.Monster.Agent.remainingDistance < 1f)
        {
            Debug.Log("�������� ����");
            stateMachine.ChangeState(stateMachine.FocusState);
            return;
        }

    }

}
