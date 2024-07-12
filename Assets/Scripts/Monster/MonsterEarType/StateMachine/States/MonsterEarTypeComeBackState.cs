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
        // �ִϸ��̼� ���� - �׶��� �Ķ���� �ؽ��� ����

    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.IsComeBack = false;

        // �ִϸ��̼� ���� - �׶��� �Ķ���� �ؽ��� ����

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
