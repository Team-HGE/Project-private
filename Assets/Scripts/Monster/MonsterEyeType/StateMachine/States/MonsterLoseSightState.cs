using UnityEngine;

public class MonsterLoseSightState : MonsterGroundState
{
    public MonsterLoseSightState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("두리번두리번");

        StartAnimation(stateMachine.Monster.AnimationData.LoseSightParameterHash);

        stateMachine.Monster.Agent.isStopped = true;
        stateMachine.Monster.IsBehavior = false;
        stateMachine.Monster.WaitForBehavior(groundData.LoseSightTransitionTime);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Monster.AnimationData.LoseSightParameterHash);
        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.Monster.StopWait();
    }

    public override void Update()
    {
        base.Update();
        
        if (!stateMachine.Monster.IsBehavior) return;
        CheckLoseSight();
    }

    public void CheckLoseSight()
    {
        if (!stateMachine.Monster.canSeePlayer)
        {
            Debug.Log("MonsterLoseSightState - 플레이어 놓침");

            if (!stateMachine.Monster.CanPatrol)
            {
                stateMachine.ChangeState(stateMachine.IdleState);
                return;
            }

            stateMachine.ChangeState(stateMachine.ComBackState);
            return;
        }
        else
        {
            stateMachine.ChangeState(stateMachine.FindState);
            return;
        }

        //if (IsInFindRange() && GetIsPlayerInFieldOfView())
        //{
        //    stateMachine.ChangeState(stateMachine.ChaseState);
        //    return;
        //}
        //else 
        //{
        //    stateMachine.ChangeState(stateMachine.ComBackState);
        //    return;
        //}
    }
}
