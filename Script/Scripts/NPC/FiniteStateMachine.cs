public interface IState
{
    void DoCheck();
    void Enter();
    void Exit();
    void LogicUpdate();
    void PhysicsUpdate();
}

public class FiniteStateMachine
{
    public IState CurrentState { get; private set; }

    public void Initialize(IState state)
    {
        if (CurrentState == null)
            CurrentState = state;

        state.Enter();
    }

    public void ChangeState(IState state)
    {
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState?.Enter();
    }
}