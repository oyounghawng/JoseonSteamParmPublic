using System.Collections;
using UnityEngine;

public class NormalNPC : NPC
{
    public Vector3 destination = Vector3.zero;

    public Coroutine co_Think = null;

    public bool isThinking = false;

    public NormalNPCIdleState IdleState { get; private set; }   
    public NormalNPCMoveState MoveState { get; private set; }
    public NormalNPCActionState ActionState { get; private set; }
    public NormalNPCTalkState TalkState { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        IdleState = new NormalNPCIdleState(this, finiteStateMachine, animator, hashIdle);
        MoveState = new NormalNPCMoveState(this, finiteStateMachine, animator, hashMove);
        ActionState = new NormalNPCActionState(this, finiteStateMachine, animator, hashAction);
        TalkState = new NormalNPCTalkState(this, finiteStateMachine, animator, hashIdle);

    }
    private void OnEnable()
    {
        TimeManager.Instance.onStartDay -= Respawn;
        TimeManager.Instance.onStartDay += Respawn;

        TimeManager.Instance.onChangedTime -= Think;
        TimeManager.Instance.onChangedTime += Think;

    }

    protected override IEnumerator Start()
    {
        yield return new WaitUntil(() => Managers.Game.isGameStart);
        finiteStateMachine.Initialize(IdleState);
    }

    public override void Interact()
    {
        base.Interact();
        isTalk = true;
        finiteStateMachine.ChangeState(TalkState);
    }

    private void Respawn()
    {
        isTalk = false;
        finiteStateMachine.ChangeState(IdleState);
        transform.position = NPCData.homePos;
    }

    public void Think(int time)
    {
        if (time % 60 == 0)
        {
            if (co_Think != null)
                StopCoroutine(co_Think);
            co_Think = StartCoroutine(CoThink());
        }
    }

    private IEnumerator CoThink()
    {
        isThinking = true;
        var node = GridManager.Instance.GetRandomNode();
        while (!node.isWalkable)
        {
            node = GridManager.Instance.GetRandomNode();
            yield return null;
        }
        destination = GridManager.Instance.GetWorldFromNode(node);

        float dist = (destination - transform.position).sqrMagnitude;

        isThinking = false;
        if (dist > distThreshold)
        {
            yield return new WaitUntil(() => !isTalk);
            isArrived = false;
            finiteStateMachine.ChangeState(MoveState);
        }
        else
        {
            yield return new WaitUntil(() => !isTalk);
            isArrived = true;
            destination = Vector3.zero;
            finiteStateMachine.ChangeState(IdleState);
        }


        yield break;
    }

}