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
        stateMachine.IsChasing = true;
        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.Monster.Agent.speed = groundData.ChaseSpeed;             
        
        // �ִϸ��̼� ����
        StartAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("�÷��̾� ���� ����");
        stateMachine.IsChasing = false;
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

        if (!stateMachine.Monster.canSeePlayer)
        {
            Debug.Log("�÷��̾� ��ħ");
            stateMachine.ChangeState(stateMachine.LoseSightState);
        }
        else 
        {
            // ���� ���� ���� - ����, ���� ����
            if (GetIsPlayerInFieldOfView() && IsInAttackRange())
            {
                stateMachine.ChangeState(stateMachine.AttackState);
                return;
            }

            stateMachine.Monster.Agent.SetDestination(stateMachine.Target.transform.position);
        }
    
        //// Ž�� ����
        //if (IsInChaseRange())
        //{
        //    stateMachine.Monster.Agent.SetDestination(stateMachine.Target.transform.position);
        //}
        //else 
        //{
        //    Debug.Log("�÷��̾� ��ħ");
        //    stateMachine.ChangeState(stateMachine.LoseSightState);
        //    return;
        //}
    }
}
