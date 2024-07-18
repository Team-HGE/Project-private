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
        Debug.Log("플레이어 추적 시작");


        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.Monster.Agent.speed = groundData.ChaseSpeed;             
        
        // 애니메이션 실행
        //StartAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);//구현 예정***
    }

    public override void Exit()
    {
        base.Exit();

        // 애니메이션 종료
        //StopAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);//구현 예정***
    }

    public override void Update()
    {
        base.Update();

        ChaseCheck();
    }

    private void ChaseCheck()
    {
        // 공격 가능 범위 - 공격, 게임 오버
        if (GetIsPlayerInFieldOfView() && IsInAttackRange())
        {
            Debug.Log("플레이어 공격 - 게임 오버");
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        //// 탐지 범위
        //if (GetIsPlayerInFieldOfView() && IsInChaseRange())
        //{
        //    stateMachine.Monster.Agent.SetDestination(stateMachine.Target.transform.position);            
        //}

        // 탐지 범위
        if (IsInChaseRange())
        {
            stateMachine.Monster.Agent.SetDestination(stateMachine.Target.transform.position);
        }
        else 
        {
            Debug.Log("플레이어 놓침");
            stateMachine.ChangeState(stateMachine.LoseSightState);
            return;

        }
    }
}
