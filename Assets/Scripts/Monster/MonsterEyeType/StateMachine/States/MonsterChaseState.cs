using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph;
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
        StartAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);
    }

    public override void Exit()
    {
        base.Exit();

        // �ִϸ��̼� ����
        StopAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);
    }

    public override void Update()
    {
        base.Update();

        ChaseCheck();
    }

    private void ChaseCheck()
    {
        //Debug.Log(Vector3.Distance(stateMachine.Monster.transform.position, stateMachine.Target.transform.position));

        // ���� ���� ���� - ����, ���� ����
        if (GetIsPlayerInFieldOfView() && IsInAttackRange())
        {           
            stateMachine.ChangeState(stateMachine.AttackState);
            return;
        }

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
