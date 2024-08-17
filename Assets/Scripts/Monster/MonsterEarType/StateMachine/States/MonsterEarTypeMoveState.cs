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

        //if (stateMachine.IsChasing || stateMachine.IsFocusNoise) return;
        if (stateMachine.Monster.Agent.isStopped) stateMachine.Monster.Agent.isStopped = false;
        stateMachine.Monster.Agent.speed = groundData.MoveSpeed;

        //Debug.Log($"소음 추적 시작 - 무브, {stateMachine.CurDestination}");

        // 애니메이션 실행
        if (!stateMachine.IsMove) StartAnimation(stateMachine.Monster.AnimationData.MoveParameterHash);
        stateMachine.IsMove = true;
        MoveToPosition(stateMachine.CurDestination, stateMachine.Monster.Data.GroundData.PlayerChasingRange);
    }

    public override void Exit()
    {
        base.Exit();
        //Debug.Log($"무브 끝");

        // 애니메이션 종료
        if (!stateMachine.IsMove) StopAnimation(stateMachine.Monster.AnimationData.MoveParameterHash);
    }

    public override void Update()
    {
        base.Update();

        if (stateMachine.Monster.Agent.pathPending) return;

        if (stateMachine.Monster.Agent.remainingDistance < 0.2f)
        {
            // 목적지에 도착
            stateMachine.IsMove = false;

            AttackToPlayer();

            //Debug.Log($"소음지역 이동 도착 {stateMachine.CurDestination}, 몬스터 위치 : {stateMachine.Monster.transform.position}");
            stateMachine.ChangeState(stateMachine.FocusState);
            return;
        }
    }
}
