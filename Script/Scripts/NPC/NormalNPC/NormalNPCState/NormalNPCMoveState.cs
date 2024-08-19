using System.Collections.Generic;
using UnityEngine;

public class NormalNPCMoveState : NPCBaseState
{
    NormalNPC normalNPC;

    private Dictionary<int, Vector3> corners = new Dictionary<int, Vector3>();

    private int cornerCount = 0;
    private Vector3 dir = Vector3.zero;
    private float dist = 0;

    private bool isCornerArrived = false;


    public NormalNPCMoveState(NormalNPC npc, FiniteStateMachine stateMachine, Animator animator, int hashKey) : base(npc, stateMachine, animator, hashKey)
    {
        normalNPC = npc;
    }

    public override void DoCheck()
    {
        base.DoCheck();

        if (animator.GetBool(npc.hashIdle)) animator.SetBool(npc.hashIdle, false);

        if (corners.Count > 0) corners.Clear();
        cornerCount = 0;
    }

    public override void Enter()
    {
        base.Enter();

        if (normalNPC.destination != Vector3.zero)
        {
            corners = GridManager.Instance.GetPath(normalNPC.transform.position, normalNPC.destination);
        }
        else
        {
            normalNPC.isArrived = true;
            stateMachine.ChangeState(normalNPC.IdleState);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!npc.isArrived)
            SetAnimationFloatParameter(dir.x, dir.y);

        if (isCornerArrived && corners.Count > cornerCount)
        {
            cornerCount++;
            isCornerArrived = false;
        }
        else if (cornerCount >= corners.Count)
        {
            npc.isArrived = true;
            stateMachine.ChangeState(normalNPC.IdleState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Vector3 npcPosition = npc.transform.position;
        if (corners.Count > cornerCount)
        {
            dir = (corners[cornerCount] - npc.transform.position).normalized;
            dist = (corners[cornerCount] - npcPosition).sqrMagnitude;

            isCornerArrived = Move(corners[cornerCount]);
        }
    }

}
