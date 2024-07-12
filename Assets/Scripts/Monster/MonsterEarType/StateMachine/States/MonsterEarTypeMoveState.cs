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

        // ��ó�� �÷��̾� ������ ����
        if (IsInAttackRange())
        {
            Debug.Log("�÷��̾� ��� - ���� ����  ");
            return;
        }

        if (Vector3.Distance(stateMachine.CurDestination, stateMachine.Monster.transform.position) < 1f)
        {
            // ������ ���� ���·� ��ȯ
            stateMachine.ChangeState(stateMachine.FocusState);
            return;
        }

        Move();
    }
}
