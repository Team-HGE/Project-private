using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterEarTypeIdleState : MonsterEarTypeGroundState
{
    public MonsterEarTypeIdleState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        // �ִϸ��̼� ���� - �׶��� �Ķ���� �ؽ��� ����
        //StartAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);//���� ����***

        stateMachine.Monster.IsBehavior = false;
        stateMachine.Monster.WaitForBehavior(stateMachine.Monster.Data.GroundData.IdleTransitionTime);

    }

    public override void Exit()
    {
        base.Exit();

        // �ִϸ��̼� ���� - �׶��� �Ķ���� �ؽ��� ����
        //StopAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);//���� ����***
    }

    public override void Update()
    {
        base.Update();

        if (!stateMachine.Monster.IsBehavior) return;
        stateMachine.ChangeState(stateMachine.PatrolState);
    }
}
