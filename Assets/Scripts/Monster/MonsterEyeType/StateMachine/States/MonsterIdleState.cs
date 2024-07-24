using UnityEngine;

public class MonsterIdleState : MonsterGroundState
{
    public MonsterIdleState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        //Debug.Log($"���̵� ����");

        // �ִϸ��̼� ����
        StartAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);

        stateMachine.Monster.IsBehavior = false;
        stateMachine.Monster.WaitForBehavior(stateMachine.Monster.Data.GroundData.IdleTransitionTime);
    }
    public override void Exit()
    {
        base.Exit();
        //Debug.Log($"���̵� ��");

        // �ִϸ��̼� ����
        StopAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);
    }

    public override void Update()
    {
        base.Update();
        if (!stateMachine.Monster.IsBehavior) return;
        stateMachine.ChangeState(stateMachine.PatrolState);
    }
}
