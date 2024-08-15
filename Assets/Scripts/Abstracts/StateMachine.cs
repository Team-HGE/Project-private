public abstract class StateMachine
{
    protected IState currentState;

    public virtual void ChangeState(IState state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }

    public void HandleInput()
    {
        currentState?.HandleInput();
    }

    public void Update()
    {
        if (!GameManager.Instance.playerDie)
        {
            currentState?.Update();
        }
    }

    public void PhysicsUpdate()
    {
        currentState?.PhysicsUpdate();
    }
}