using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MainNPC : NPC, IInteractable
{
    private List<NPCRoutine> routines = new List<NPCRoutine>();
    private List<NPCRoutine> dayRoutine = new List<NPCRoutine>();

    [SerializeField]
    private NPCRoutine currentRoutine = null;
    public NPCRoutine CurrentRoutine
    {
        get => currentRoutine;
        set
        {
            currentRoutine = value;
            isArrived = IsArrived(currentRoutine.destination);
        }
    }

    #region Main NPC State
    public MainNPCIdleState IdleState { get; private set; }
    public MainNPCMoveState MoveState { get; private set; }
    public MainNPCTalkState TalkState { get; private set; }
    public MainNPCActionState ActionState { get; private set; }

    #endregion

    protected override void Awake()
    {
        base.Awake();
        IdleState = new MainNPCIdleState(this, finiteStateMachine, animator, hashIdle);
        MoveState = new MainNPCMoveState(this, finiteStateMachine, animator, hashMove);
        TalkState = new MainNPCTalkState(this, finiteStateMachine, animator, hashIdle);
        ActionState = new MainNPCActionState(this, finiteStateMachine, animator, hashAction);
    }

    private void OnEnable()
    {
        // 시작 시 자신의 집에서 시작
        TimeManager.Instance.onStartDay -= Respawn;
        TimeManager.Instance.onStartDay += Respawn;

        // 날이 바뀔 때 마다 해당 Day의 Routine을 가져옴
        TimeManager.Instance.onChangedDay -= GetDayRoutine;
        TimeManager.Instance.onChangedDay += GetDayRoutine;

        // 시간이 지날 때 마다 루틴을 점검
        TimeManager.Instance.onChangedTime -= DoRoutine;
        TimeManager.Instance.onChangedTime += DoRoutine;
    }

    protected override IEnumerator Start()
    {
        yield return new WaitUntil(() => Managers.Data.NPCRoutineDict != null && Managers.Data.NPCRoutineDict.Count > 0);

        if (Managers.Data.NPCRoutineDict.TryGetValue(gameObject.name, out var retrievedRoutines))
        {
            routines = retrievedRoutines;
        }
        else
        {
            routines = new List<NPCRoutine>();
        }

        yield return new WaitUntil(() => GridManager.Instance.isInit);

        finiteStateMachine.Initialize(IdleState);
    }

    private void Respawn()
    {
        isTalk = false;
        finiteStateMachine.ChangeState(IdleState);
        transform.position = NPCData.homePos;
    }


    public override void Interact()
    {
        base.Interact();

        isTalk = true;
        finiteStateMachine.ChangeState(TalkState);
    }

    private void GetDayRoutine(int day)
    {
        if (routines.Count > 0)
        {
            dayRoutine = routines.FindAll(x => x.dayofweek == TimeManager.Instance.Day % 7);
        }
    }
    private async void DoRoutine(int time)
    {
        while (dayRoutine.Count < 0 && dayRoutine == null) await Task.Yield();
        if (dayRoutine != null && routines.Count > 0 && routines != null)
        {
            foreach (var rt in dayRoutine)
            {
                if (rt.endTime >= time)
                {
                    if (CurrentRoutine != rt)
                        CurrentRoutine = rt;
                    break;
                }
            }
        }
    }
}
