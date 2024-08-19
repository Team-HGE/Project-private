public class MonsterComeBackState : MonsterGroundState
{
    public MonsterComeBackState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

        if (!stateMachine.Monster.CanComeBack)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
            return;
        }

        StartAnimation(stateMachine.Monster.AnimationData.ComeBackParameterHash);

        stateMachine.IsComeBack = true;
        stateMachine.Monster.Agent.isStopped = false;
        stateMachine.Monster.Agent.speed = groundData.PatrolSpeed;
        stateMachine.Monster.Agent.SetDestination(stateMachine.StartPosition);
    }

    public override void Exit()
    {
        base.Exit();
        stateMachine.IsComeBack = false;
        StopAnimation(stateMachine.Monster.AnimationData.ComeBackParameterHash);
    }

    public override void Update()
    {
        base.Update();
        //RotateToPlayer();

        if (stateMachine.Monster.Agent.pathPending) return;

        if (stateMachine.Monster.Agent.remainingDistance < 0.2f)
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
    }
}
