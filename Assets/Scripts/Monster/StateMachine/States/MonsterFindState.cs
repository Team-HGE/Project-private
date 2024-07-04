using UnityEngine;

public class MonsterFindState : MonsterGroundState
{
    public MonsterFindState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateMachine.Monster.Agent.isStopped = true;
        stateMachine.Monster.IsBehavior = false;

        // 플레이어 바라보기
        Rotate(GetMovementDirection());
        stateMachine.Monster.WaitForBehavior(groundData.FindTransitionTime);      
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        Rotate(GetMovementDirection());
        if (!stateMachine.Monster.IsBehavior) return;
        FindCheck();
    }

    private void FindCheck()
    {
        // 공격 범위 안이면 공격 - 게임 오버
        if (IsInAttackRange() && GetIsPlayerInFieldOfView())
        {
            // 플레이어 공격
            Debug.Log("플레이어 공격 - 게임 오버");

            stateMachine.Monster.IsBehavior = false;// 임시코드 게임 오버 구현 후 수정할 것***
            return;
        }

        if (IsInChaseRange() && GetIsPlayerInFieldOfView())
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }
        else 
        {
            stateMachine.ChangeState(stateMachine.LoseSightState);
            return;
        }
    }
}
