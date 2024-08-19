using UnityEngine;

public class NormalNPCActionState : NPCBaseState
{
    NormalNPC normalNPC;
    public NormalNPCActionState(NormalNPC npc, FiniteStateMachine stateMachine, Animator animator, int hashKey) : base(npc, stateMachine, animator, hashKey)
    {
        normalNPC = npc;
    }

    public override void DoCheck()
    {
        base.DoCheck();
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
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }
}
