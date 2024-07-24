using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterGroundState
{
    public MonsterAttackState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // �ִϸ��̼� ����
        StartAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);        
        stateMachine.Monster.Agent.isStopped = true;
        Debug.Log("�÷��̾� ���� - ���� ����");

    }

    public override void Exit()
    {
        base.Exit();

        // �ִϸ��̼� ����
        StopAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);
    }
}
