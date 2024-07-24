using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterEarTypeMoveState : MonsterEarTypeGroundState
{
    public MonsterEarTypeMoveState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (stateMachine.IsChasing || stateMachine.IsFocusNoise) return;

        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.Monster.Agent.speed = groundData.MoveSpeed;

        //Debug.Log($"소음 추적 시작 - 무브, {stateMachine.CurDestination}");         

        stateMachine.Monster.Agent.SetDestination(stateMachine.CurDestination);

        // 애니메이션 실행
        if (!stateMachine.IsMove) StartAnimation(stateMachine.Monster.AnimationData.MoveParameterHash);
        StartAnimation(stateMachine.Monster.AnimationData.MoveParameterHash);
        stateMachine.IsMove = true;



    }

    public override void Exit()
    {
        base.Exit();

        //stateMachine.IsMove = false;

        // 애니메이션 종료
        if (!stateMachine.IsMove) StopAnimation(stateMachine.Monster.AnimationData.MoveParameterHash);

    }

    public override void Update()
    {
        base.Update();
        
        if (stateMachine.Monster.Agent.remainingDistance < 1f)
        {
            if (IsInAttackRange())
            {
                stateMachine.ChangeState(stateMachine.AttackState);
                return;
            }


            stateMachine.IsMove = false;
            //Debug.Log("소음지역 도착");
            stateMachine.ChangeState(stateMachine.FocusState);
            return;
        }

    }

}
