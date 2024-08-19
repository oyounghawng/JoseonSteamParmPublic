using UnityEngine;

public abstract class PlayerAnimationState
{
    protected PlayerAnimationController controller;
    private Animator animator;
    private int hashParameter;
    public PlayerAnimationState(PlayerAnimationController controller, Animator animator, int hashParameter)
    {
        this.controller = controller;
        this.animator = animator;
        this.hashParameter = hashParameter;
    }

    public virtual void Enter()
    {
        animator.SetBool(hashParameter, true);
    }
    public virtual void Exit()
    {
        animator.SetBool(hashParameter, false);
    }

    public virtual void LogicUpdate()
    {

    }

}
