using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeAttackState : MonsterEarTypeGroundState
{
    public MonsterEarTypeAttackState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {

    }

    public override void Enter()
    {
        base.Enter();
        stateMachine.IsAttack = true;
        stateMachine.Monster.Agent.isStopped = true;
        Debug.Log("�÷��̾� ��� - ���� ����");
        JumpScareManager.Instance.OnJumpScare(stateMachine.Monster.monsterTransform, JumpScareType.EyeTypeMonster, stateMachine.Monster.monsterEyeTransform);
        // �ִϸ��̼� ����
        StartAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.IsAttack = false;
        stateMachine.Monster.Agent.isStopped = false;


        // �ִϸ��̼� ����
        StopAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);

    }
}
