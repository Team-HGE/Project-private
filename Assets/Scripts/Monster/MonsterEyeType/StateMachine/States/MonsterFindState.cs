using UnityEngine;

public class MonsterFindState : MonsterGroundState
{
    public MonsterFindState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("MonsterFindState - 플레이어 발견 주시 시작");
        stateMachine.IsFind = true;
        stateMachine.Monster.Agent.isStopped = true;
        stateMachine.Monster.IsBehavior = false;

        StartAnimation(stateMachine.Monster.AnimationData.FindParameterHash);

        stateMachine.Monster.WaitForBehavior(groundData.FindTransitionTime);
    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("MonsterFindState - 플레이어 발견 주시 종료");
        stateMachine.IsFind = false;
        StopAnimation(stateMachine.Monster.AnimationData.FindParameterHash);
        stateMachine.Monster.StopWait();
    }

    public override void Update()
    {
        base.Update();
        Rotate(GetMovementDirection());
        if (!stateMachine.Monster.IsBehavior) return;
        //Debug.Log("MonsterFindState - 행동시작");

        FindCheck();
    }

    private void FindCheck()
    {
        if (!stateMachine.Monster.canSeePlayer)
        {
            Debug.Log("플레이어 놓침");
            stateMachine.ChangeState(stateMachine.LoseSightState);
        }
        else 
        {
            // 공격 범위 안이면 공격 - 게임 오버
            if (IsInAttackRange() && GetIsPlayerInFieldOfView())
            {
                stateMachine.ChangeState(stateMachine.AttackState);
                return;
            }

            Debug.Log("플레이어 추적");
            stateMachine.ChangeState(stateMachine.ChaseState);
        }
    }
}
