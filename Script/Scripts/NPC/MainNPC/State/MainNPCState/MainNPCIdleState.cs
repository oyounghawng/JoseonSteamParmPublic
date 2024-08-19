using UnityEngine;

public class MainNPCIdleState : NPCBaseState
{
    private MainNPC mainNPC;

    float x = 0;
    float y = 0;

    float distance;

    public MainNPCIdleState(MainNPC npc, FiniteStateMachine stateMachine, Animator animator, int hashKey) : base(npc, stateMachine, animator, hashKey)
    {
        mainNPC = npc;
    }


    public override void Enter()
    {
        base.Enter();

        x = animator.GetFloat(npc.hashXVelocity);
        y = animator.GetFloat(npc.hashYVelocity);

        if (mainNPC.CurrentRoutine.direction.x != x || mainNPC.CurrentRoutine.direction.y != y) 
            SetAnimationFloatParameter(mainNPC.CurrentRoutine.direction.x, mainNPC.CurrentRoutine.direction.y);
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override void Exit()
    {
        base.Exit();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isFinishExit) return;

        if (Time.time - lastCheckTime > checkRate)
        {
            #region Transition
            // 루틴이 존재할 때
            if(mainNPC.CurrentRoutine != null)
            {
                /* Idle -> Action */

                if (npc.isArrived)
                {
                    if (mainNPC.CurrentRoutine.behavior != Define.NPCBehavior.Look)
                    {
                        stateMachine.ChangeState(mainNPC.ActionState);
                    }
                }
                else
                {
                    /* Idle -> Move */
                    if (distance > npc.distThreshold)
                    {
                        npc.isArrived = false;
                        stateMachine.ChangeState(mainNPC.MoveState);
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

        if (!npc.isArrived)
            distance = (mainNPC.CurrentRoutine.destination - mainNPC.transform.position).sqrMagnitude;
    }
}
