public class MonsterLoseSightState : MonsterGroundState
{
    public MonsterLoseSightState(MonsterStateMachine monsterStateMachine) : base(monsterStateMachine)
    {
    }

    public override void Enter()
    {
        base.Enter();

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
        CheckLoseSight();
    }

    public void CheckLoseSight()
    {
        if (IsInFindRange() && GetIsPlayerInFieldOfView())
        {
            stateMachine.ChangeState(stateMachine.ChaseState);
            return;
        }
        else 
        {
            stateMachine.ChangeState(stateMachine.ComBackState);
            return;
        }
    }
}
