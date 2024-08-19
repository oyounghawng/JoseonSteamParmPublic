using UnityEngine;
using static TutorialController;

public enum TutorialTpye
{
    None,
    Seed,
    Harvest,
}

public class TutorialAction : TutorialBase
{
    [SerializeField]
    private Transform triggerObject;

    [SerializeField]
    private TutorialTpye type;

    public GameObject potatoSeed;
    public GameObject targetObject;

    private bool isSuccess = false;

    private TutorialCheck curCheck;
    public override void Enter()
    {
        triggerObject.gameObject.SetActive(true);
        (Managers.Scene.CurrentScene as GameScene).player.GetComponent<PlayerInputController>().isMovementRestricted = false;
        TutorialController controller = GetComponentInParent<TutorialController>();

        if (type == TutorialTpye.Seed)
        {
            (Managers.Scene.CurrentScene as GameScene).player.AddItem(potatoSeed.GetComponent<ItemObject>().itemObjectData);
        }
        else if (type == TutorialTpye.Harvest)
        {
            GridManager.Instance.FullGrow(triggerObject.transform.position);
        }


        if (controller.tutorialQueue.Count > 0)
        {
            // 큐에서 함수를 하나 가져와서 실행
            curCheck = controller.tutorialQueue.Dequeue();
        }
    }

    public override void Execute(TutorialController controller)
    {
        if (type == TutorialTpye.Harvest)
        {
            if (curCheck(targetObject))
            {
                controller.SetNextTutorial();
            }
        }
        else
        {
            if (curCheck(triggerObject.gameObject))
            {
                controller.SetNextTutorial();
            }
        }
    }

    public override void Exit()
    {
        triggerObject.gameObject.SetActive(false);
    }
}
