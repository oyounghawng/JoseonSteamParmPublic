using System.Collections;

public class VillageManagerNPC : NPC
{
    protected override IEnumerator Start()
    {
        yield return null;
    }
    public async override void Interact()
    {
        base.Interact();
        if (!isTalk)
        {
            isTalk = true;
            UI_Dialogue dialogue = await Managers.UI.ShowTaskPopupUI<UI_Dialogue>();
            dialogue.SetDialogue(DialogueContainer.dialogueGroup.Find(x => x.key.Equals("Interaction_Context")), NPCData.portraits);
            dialogue.onEndDialogue += EndTalk;
        }
    }
    private void EndTalk()
    {
        isTalk = false;
    }
}
