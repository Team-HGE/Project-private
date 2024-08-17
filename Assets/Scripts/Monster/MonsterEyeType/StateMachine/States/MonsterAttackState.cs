using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttackState : MonsterGroundState
{
    public MonsterAttackState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();

        if (GameManager.Instance.nowPlayCutScene)
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }

        stateMachine.IsAttack = true;
        stateMachine.Monster.Agent.isStopped = true;
        
        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);

        Debug.Log("플레이어 공격 - 게임 오버");
        GameManager.Instance.playerDie = true;
        // 점프스퀘어
        GameManager.Instance.jumpScareManager.PlayJumpScare(JumpScareType.EyeTypeMonster);
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
