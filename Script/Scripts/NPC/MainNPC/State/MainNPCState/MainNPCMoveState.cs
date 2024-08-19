using System.Collections.Generic;
using UnityEngine;

public class MainNPCMoveState : NPCBaseState
{
    private MainNPC mainNPC;
    private Dictionary<int, Vector3> corners = new Dictionary<int, Vector3>();

    private int cornerCount = 0;
    private float dist;
    private Vector3 dir;

    private bool isCornerArrived = false;


    public MainNPCMoveState(MainNPC npc, FiniteStateMachine stateMachine, Animator animator, int hashKey) : base(npc, stateMachine, animator, hashKey)
    {
        mainNPC = npc;
    }

    public override void DoCheck()
    {
        base.DoCheck();
        if (cornerCount > 0) cornerCount = 0;
    }

    public override void Enter()
    {
        base.Enter();

        corners = GridManager.Instance?.GetPath(mainNPC.transform.position, mainNPC.CurrentRoutine.destination);
    }

    public override void Exit()
    {
        base.Exit();
        cornerCount = 0;
        corners.Clear();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (isFinishExit) return;

        if (!npc.isArrived)
            SetAnimationFloatParameter(dir.x, dir.y);

        if (isCornerArrived && corners.Count > cornerCount)
        {
            cornerCount++;
            isCornerArrived = false;
        }
        else if (corners.Count <= cornerCount)
        {
            npc.isArrived = true;

            stateMachine.ChangeState(mainNPC.IdleState);
        }

    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Vector3 npcPosition = npc.transform.position;

        if (cornerCount < corners.Count)
        {
            dir = (corners[cornerCount] - npcPosition).normalized;
            dist = (corners[cornerCount] - npcPosition).sqrMagnitude;

            isCornerArrived = Move(corners[cornerCount]);
        }
    }
}
