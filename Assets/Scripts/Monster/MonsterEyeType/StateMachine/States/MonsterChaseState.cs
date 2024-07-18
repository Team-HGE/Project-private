using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterChaseState : MonsterGroundState
{
    public MonsterChaseState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("�÷��̾� ���� ����");


        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.Monster.Agent.speed = groundData.ChaseSpeed;             
        
        // �ִϸ��̼� ����
        //StartAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);//���� ����***
    }

    public override void Exit()
    {
        base.Exit();

        // �ִϸ��̼� ����
        //StopAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);//���� ����***
    }

    public override void Update()
    {
        base.Update();

        ChaseCheck();
    }

    private void ChaseCheck()
    {
        // ���� ���� ���� - ����, ���� ����
        if (GetIsPlayerInFieldOfView() && IsInAttackRange())
        {
            Debug.Log("�÷��̾� ���� - ���� ����");
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        //// Ž�� ����
        //if (GetIsPlayerInFieldOfView() && IsInChaseRange())
        //{
        //    stateMachine.Monster.Agent.SetDestination(stateMachine.Target.transform.position);            
        //}

        // Ž�� ����
        if (IsInChaseRange())
        {
            stateMachine.Monster.Agent.SetDestination(stateMachine.Target.transform.position);
        }
        else 
        {
            Debug.Log("�÷��̾� ��ħ");
            stateMachine.ChangeState(stateMachine.LoseSightState);
            return;

        }
    }
}
