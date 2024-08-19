public class BaseState : IState
{
    public virtual void DoCheck()
    {

    }
    public virtual void Enter()
    {
        DoCheck();
    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate()
    {

    }
}