using UnityEngine;

public class MonsterAttackState : MonsterGroundState
{
    public MonsterAttackState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }
    public override void Enter()
    {
        base.Enter();

        if (GameManager.Instance.NowPlayCutScene)
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }

        stateMachine.IsAttack = true;
        stateMachine.Monster.Agent.isStopped = true;
        
        // 애니메이션 실행
        StartAnimation(stateMachine.Monster.AnimationData.AttackParameterHash);

        Debug.Log("플레이어 공격 - 게임 오버");
        // 점프스퀘어
        GameManager.Instance.jumpScareManager.PlayJumpScare(JumpScareType.EyeTypeMonster);
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
