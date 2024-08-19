using UnityEngine;

public class TutorialFishAction : TutorialBase
{
    public override void Enter()
    {
        (Managers.Scene.CurrentScene as GameScene).player.GetComponent<PlayerInputController>().isMovementRestricted = false;
    }

    public override void Execute(TutorialController controller)
    {
        if (HasItem())
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {

    }

    private bool HasItem()
    {
        if ((Managers.Scene.CurrentScene as GameScene).player.HasPlyerItem(Define.ItemType.FishingItem))
        {
            return true;
        }
        return false;
    }
}
