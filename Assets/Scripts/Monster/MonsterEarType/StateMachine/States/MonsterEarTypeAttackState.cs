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

        if (GameManager.Instance.playerDie || GameManager.Instance.nowPlayCutScene)
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }

        stateMachine.IsAttack = true;
        stateMachine.Monster.Agent.isStopped = true;
        
        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);

        Debug.Log("플레이어 사망 - 게임 오버");
        GameManager.Instance.playerDie = true;
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.IsAttack = false;
        stateMachine.Monster.Agent.isStopped = false;

        // 애니메이션 종료
        StopAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);
    }
}
