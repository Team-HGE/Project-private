public class MonsterIdleState : MonsterGroundState
{
    public MonsterIdleState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // �ִϸ��̼� ����
        //StartAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);//���� ����***
    }
    public override void Exit()
    {
        base.Exit();
        // �ִϸ��̼� ����
        //StopAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);//���� ����***
    }
}
