using UnityEngine;

public class NormalNPCIdleState : NPCBaseState
{
    private NormalNPC normalNPC;

    public NormalNPCIdleState(NormalNPC npc, FiniteStateMachine stateMachine, Animator animator, int hashKey) : base(npc, stateMachine, animator, hashKey)
    {
        normalNPC = npc;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        if (animator.GetBool(npc.hashMove)) animator.SetBool(npc.hashMove, false);

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (Time.time - lastCheckTime > checkRate)
        {
            /* Transition Idle -> Move */
            if (npc.isArrived)
            {
                // TODO : 액션이냐 Look이냐
            }
            else if (!normalNPC.isThinking && !npc.isArrived)
            {
                stateMachine.ChangeState(normalNPC.MoveState);
            }

            lastCheckTime = Time.time;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

}