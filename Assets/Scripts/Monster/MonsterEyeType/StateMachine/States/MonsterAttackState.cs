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
        JumpScareManager.Instance.OnJumpScare(stateMachine.Monster.monsterTransform, JumpScareType.EyeTypeMonster, stateMachine.Monster.monsterEyeTransform);

        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);        
        stateMachine.Monster.Agent.isStopped = true;
        Debug.Log("플레이어 공격 - 게임 오버");

    }

    public override void Exit()
    {
        base.Exit();

        // 애니메이션 종료
        StopAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);
    }
}
