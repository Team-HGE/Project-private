using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MonsterEarTypeChaseState : MonsterEarTypeGroundState
{
    public MonsterEarTypeChaseState(MonsterEarTypeStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //Debug.Log("chace 시작");
        //stateMachine.Monster.Agent.isStopped = true;
        stateMachine.Monster.Agent.speed = groundData.ChaseSpeed;       
        // 애니메이션 실행
        if (!stateMachine.IsChasing) StartAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);
        stateMachine.IsChasing = true;
        //StartAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);

        MoveToPosition(stateMachine.CurDestination, stateMachine.Monster.Data.GroundData.PlayerChasingRange);
    }

    public override void Exit()
    {
        base.Exit();

        //Debug.Log("chace 끝");
        //stateMachine.IsChasing = false;
        // 애니메이션 종료
        if (!stateMachine.IsChasing) StopAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);
        //StopAnimation(stateMachine.Monster.AnimationData.ChaseParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Monster.Agent.pathPending) return;

        if (stateMachine.Monster.Agent.remainingDistance < 0.2f)
        {
            // 목적지에 도착
            stateMachine.IsChasing = false;

            if (IsInAttackRange())
            {
                stateMachine.ChangeState(stateMachine.AttackState);
                return;
            }

            //Debug.Log($"소음지역 추적 도착 {stateMachine.CurDestination}, 몬스터 위치 : {stateMachine.Monster.transform.position}");
            stateMachine.ChangeState(stateMachine.FocusState);
            return;
        }
    }
}
