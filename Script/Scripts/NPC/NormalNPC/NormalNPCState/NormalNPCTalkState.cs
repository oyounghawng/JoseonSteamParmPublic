using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NormalNPCTalkState : NPCBaseState
{
    NormalNPC normalNPC;
    UI_Dialogue dialogueUI = null;

    public NormalNPCTalkState(NormalNPC npc, FiniteStateMachine stateMachine, Animator animator, int hashKey) : base(npc, stateMachine, animator, hashKey)
    {
        normalNPC = npc;
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



        var dialogueMap = new Dictionary<(Define.Gender, Define.NPCRole), string>
        {
            { (Define.Gender.Male, Define.NPCRole.Farmer), "남자 농부" },
            { (Define.Gender.Male, Define.NPCRole.Fisher), "남자 어부" },
            { (Define.Gender.Female, Define.NPCRole.Farmer), "여자 농부" },
            { (Define.Gender.Female, Define.NPCRole.Fisher), "여자 어부" }
        };

        // NPC의 성별과 역할에 따라 대화 내용을 가져오기
        if (dialogueMap.TryGetValue((npc.NPCData.gender, npc.NPCData.role), out var displayName))
        {
            var dt = (NPCDialogue)npc.DialogueContainer.dialogueGroup
                .Where(x => x.key.Equals("Interaction_Context") && x.displayName.Equals(displayName))
                .FirstOrDefault();

            dialogueUI = await UIManager.Instance.ShowTaskPopupUI<UI_Dialogue>();
            dialogueUI.onEndDialogue += TransitionToIdle;
            dialogueUI.SetDialogue(dt, npc.NPCData.portraits);
        }
    }

    public override void Exit()
    {
        base.Exit();
        npc.isTalk = false;

    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void TransitionToIdle()
    {
        stateMachine.ChangeState(normalNPC.IdleState);
    }
}
