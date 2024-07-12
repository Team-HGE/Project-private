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

        // �÷��̾� �ٶ󺸱�
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
        // ���� ���� ���̸� ���� - ���� ����
        if (IsInAttackRange() && GetIsPlayerInFieldOfView())
        {
            // �÷��̾� ����
            Debug.Log("�÷��̾� ���� - ���� ����");

            stateMachine.Monster.IsBehavior = false;// �ӽ��ڵ� ���� ���� ���� �� ������ ��***
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
