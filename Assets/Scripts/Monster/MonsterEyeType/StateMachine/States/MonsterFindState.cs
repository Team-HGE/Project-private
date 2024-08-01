using UnityEngine;

public class MonsterFindState : MonsterGroundState
{
    public MonsterFindState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("MonsterFindState - �÷��̾� �߰� �ֽ� ����");
        stateMachine.IsFocus = true;
        stateMachine.Monster.Agent.isStopped = true;
        stateMachine.Monster.IsBehavior = false;

        // �÷��̾� �ٶ󺸱�
        //Rotate(GetMovementDirection());
        StartAnimation(stateMachine.Monster.AnimationData.FindParameterHash);

        stateMachine.Monster.WaitForBehavior(groundData.FindTransitionTime);

    }

    public override void Exit()
    {
        base.Exit();
        Debug.Log("MonsterFindState - �÷��̾� �߰� �ֽ� ����");
        stateMachine.IsFocus = false;
        StopAnimation(stateMachine.Monster.AnimationData.FindParameterHash);

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
        if (!stateMachine.Monster.canSeePlayer)
        {
            Debug.Log("�÷��̾� ��ħ");
            stateMachine.ChangeState(stateMachine.LoseSightState);
        }
        else 
        {
            // ���� ���� ���̸� ���� - ���� ����
            if (IsInAttackRange() && GetIsPlayerInFieldOfView())
            {
                // �÷��̾� ����
                //Debug.Log("�÷��̾� ���� - ���� ����");
                //stateMachine.Monster.IsBehavior = false;// �ӽ��ڵ� ���� ���� ���� �� ������ ��***

                stateMachine.ChangeState(stateMachine.AttackState);
                return;
            }

            Debug.Log("�÷��̾� ����");
            stateMachine.ChangeState(stateMachine.ChaseState);
        }




        //if (IsInChaseRange() && GetIsPlayerInFieldOfView())
        //{
        //    stateMachine.ChangeState(stateMachine.ChaseState);
        //    return;
        //}
        //else 
        //{
        //    stateMachine.ChangeState(stateMachine.LoseSightState);
        //    return;
        //}
    }
}
