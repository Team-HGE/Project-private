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
        // 애니메이션 실행
        stateMachine.IsAttack = true;
        stateMachine.Monster.Agent.isStopped = true;
        StartAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);
        Debug.Log("플레이어 공격 - 게임 오버");

        // 점프스퀘어
        JumpScareManager.Instance.OnJumpScare(stateMachine.Monster.monsterTransform, JumpScareType.EyeTypeMonster, stateMachine.Monster.monsterEyeTransform);      
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
