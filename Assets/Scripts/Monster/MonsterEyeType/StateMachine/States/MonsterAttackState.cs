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

        JumpScareManager.Instance.OnJumpScare(stateMachine.Monster.monsterTransform, JumpScareType.EyeTypeMonster, stateMachine.Monster.monsterEyeTransform);      
    }

    public override void Exit()
    {
        base.Exit();

        // �ִϸ��̼� ����
        StopAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);
    }
}
