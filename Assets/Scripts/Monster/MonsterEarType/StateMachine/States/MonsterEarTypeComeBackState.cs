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
        stateMachine.Monster.Agent.speed = groundData.ComebackSpeed;
        stateMachine.Monster.Agent.SetDestination(stateMachine.StartPosition);

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

        if (stateMachine.Monster.Agent.remainingDistance < 1f)
        {
            Debug.Log("���� �Ϸ�");
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
