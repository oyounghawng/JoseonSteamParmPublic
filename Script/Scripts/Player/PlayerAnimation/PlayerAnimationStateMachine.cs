public class PlayerAnimationStateMachine
{
    public PlayerAnimationState CurrentState { get; private set; }

    public void Initialize(PlayerAnimationState state)
    {
        CurrentState = state;
        CurrentState?.Enter();
    }

    public void ChangeState(PlayerAnimationState state)
    {
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState?.Enter();
    }
}
