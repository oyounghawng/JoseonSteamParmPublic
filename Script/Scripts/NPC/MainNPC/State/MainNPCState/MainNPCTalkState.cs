using System;
using UnityEngine;

public class MainNPCTalkState : NPCBaseState
{
    MainNPC mainNPC;
    UI_Dialogue dialogueUI = null;

    public MainNPCTalkState(MainNPC npc, FiniteStateMachine stateMachine, Animator animator, int hashKey) : base(npc, stateMachine, animator, hashKey)
    {
        mainNPC = npc;
    }

    public override void DoCheck()
    {
        base.DoCheck();
    }

    public override async void Enter()
    {
        base.Enter();

        Vector3 dir = ((SceneManagerEx.Instance.CurrentScene as GameScene).player.transform.position - npc.transform.position).normalized;

        SetAnimationFloatParameter(dir.x, dir.y);

        dialogueUI = await UIManager.Instance.ShowTaskPopupUI<UI_Dialogue>();
        dialogueUI.onEndDialogue += TransitionToIdle;

        // ��Ȳ�� ���� �پ��� ��ȭ�� ����
        // ������ �ƴ� ���
        if (!npc.isManageShop)
        {
            // ù �Ұ��� ���
            if (npc.isIntroduced)
            {
                // �Ұ� Dialogue�� ���
                var dt = npc.DialogueContainer.dialogueGroup.Find(x => x.key == "Introduction");
                dialogueUI.SetDialogue(dt, npc.NPCData.portraits);
                npc.isIntroduced = false;
            }
            else if (!npc.isIntroduced && !npc.isNormalTalk)
            {
                string season = Enum.GetName(typeof(Define.Season), TimeManager.Instance.Season);
                string day = Enum.GetName(typeof(Define.Day), TimeManager.Instance.Day % 7);

                var dt = npc.DialogueContainer.dialogueGroup.Find(x => x.key == $"{season}_{day}");
                dialogueUI.SetDialogue(dt, npc.NPCData.portraits);
                npc.isNormalTalk = true;
            }
            else if (!npc.isIntroduced && npc.isNormalTalk)
            {
                var dt = npc.DialogueContainer.dialogueGroup.Find(x => x.key.Equals("Normal_Context"));
                dialogueUI.SetDialogue(dt, npc.NPCData.portraits);
            }
        }
        // ������ ���
        else
        {
            if (npc.isNormalTalk)
            {
                var dt = npc.DialogueContainer.dialogueGroup.Find(x => x.key.Equals("Normal_Context"));
                dialogueUI.SetDialogue(dt);
            }
        }

    }

    public override void Exit()
    {
        base.Exit();
        npc.isTalk = false;
        dialogueUI = null;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    protected override void SetAnimationFloatParameter(float xVeloctiy, float yVelocity)
    {
        base.SetAnimationFloatParameter(xVeloctiy, yVelocity);
    }

    private void TransitionToIdle()
    {
        stateMachine.ChangeState(mainNPC.IdleState);
    }
}
