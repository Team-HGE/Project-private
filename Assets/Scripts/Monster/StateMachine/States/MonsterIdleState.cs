public class MonsterIdleState : MonsterGroundState
{
    public MonsterIdleState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();
        // 애니메이션 실행
        //StartAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);//구현 예정***
    }
    public override void Exit()
    {
        base.Exit();
        // 애니메이션 종료
        //StopAnimation(stateMachine.Monster.AnimationData.IdleParameterHash);//구현 예정***
    }
}
