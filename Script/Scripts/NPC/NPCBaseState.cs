using UnityEngine;

public class NPCBaseState : BaseState
{
    protected NPC npc = null;
    protected FiniteStateMachine stateMachine = null;
    protected Animator animator = null;

    protected bool isFinishExit = false;

    protected int hashKey = -1;
    protected bool canActive = false;


    protected float lastCheckTime;
    protected float checkRate = 0.05f;

    public NPCBaseState(NPC npc, FiniteStateMachine stateMachine, Animator animator, int hashKey)
    {
        this.npc = npc;
        this.stateMachine = stateMachine;
        this.animator = animator;
        this.hashKey = hashKey;
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
        isFinishExit = false;
        animator.SetBool(hashKey, true);
        // isArrived = IsArrived(npc.routines[npc.rtIndex].destination);
    }

    public override void Exit()
    {
        base.Exit();
        isFinishExit = true;

        animator.SetBool(hashKey, false);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isFinishExit)
            return;

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected virtual void SetAnimationFloatParameter(float xVeloctiy, float yVelocity)
    {
        animator.SetFloat(npc.hashXVelocity, xVeloctiy);
        animator.SetFloat(npc.hashYVelocity, yVelocity);
    }

    protected bool Move(Vector3 destination)
    {
        npc.transform.position = Vector3.MoveTowards(npc.transform.position, destination, 0.05f);

        return npc.IsArrived(destination);
    }
}