using UnityEngine;

public class MainNPCActionState : NPCBaseState
{
    MainNPC mainNPC;

    float distance;


    public MainNPCActionState(MainNPC npc, FiniteStateMachine stateMachine, Animator animator, int hashKey) : base(npc, stateMachine, animator, hashKey)
    {
        mainNPC = npc;
    }
    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Enter()
    {
        base.Enter();
        animator.SetInteger(npc.hashBehavior, (int)mainNPC.CurrentRoutine.behavior);
    }

    public override void Exit()
    {
        base.Exit();

        animator.SetInteger(npc.hashBehavior, 0);
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isFinishExit) return;
        if (Time.time - lastCheckTime > checkRate)
        {
            #region Transition
            // 루틴이 존재할 때

            if (mainNPC.CurrentRoutine != null)
            {
                /* Idle -> Action */
                if (!npc.isArrived)
                    stateMachine.ChangeState(mainNPC.MoveState);
                else
                {
                    if (mainNPC.CurrentRoutine.behavior == Define.NPCBehavior.Look)
                    {
                        stateMachine.ChangeState(mainNPC.IdleState);
                    }
                }
            }
            #endregion
            lastCheckTime = Time.time;
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (isFinishExit) return;
    }
}