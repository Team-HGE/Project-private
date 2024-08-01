using UnityEngine;

public class MonsterLoseSightState : MonsterGroundState
{
    public MonsterLoseSightState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("�θ����θ���");

        StartAnimation(stateMachine.Monster.AnimationData.LoseSightParameterHash);

        stateMachine.Monster.Agent.isStopped = true;
        stateMachine.Monster.IsBehavior = false;
        stateMachine.Monster.WaitForBehavior(groundData.LoseSightTransitionTime);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Monster.AnimationData.LoseSightParameterHash);

    }

    public override void Update()
    {
        base.Update();
        
        if (!stateMachine.Monster.IsBehavior) return;
        // �ڷ�ƾ �����Ұ�*****
        CheckLoseSight();
    }

    public void CheckLoseSight()
    {
        if (!stateMachine.Monster.canSeePlayer)
        {
            Debug.Log("MonsterLoseSightState - �÷��̾� ��ħ");

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
            stateMachine.ChangeState(stateMachine.ChaseState);
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
