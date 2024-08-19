using UnityEngine;

public class TutorialTrigger : TutorialBase
{
    Player player;
    [SerializeField]
    private Transform triggerObject;

    public bool isTrigger { set; get; } = false;

    public override void Enter()
    {
        // �÷��̾� �̵� ����
        player = (Managers.Scene.CurrentScene as GameScene).player;
        player.GetComponent<PlayerInputController>().isMovementRestricted = false;
        // Trigger ������Ʈ Ȱ��ȭ
        triggerObject.gameObject.SetActive(true);
    }

    public override void Execute(TutorialController controller)
    {
        /*
		/// �Ÿ� ����
		if ( (triggerObject.position - playerController.transform.position).sqrMagnitude < 0.1f )
		{
			controller.SetNextTutorial();
		}*/

        /// �浹 ����
        // TutorialTrigger ������Ʈ�� ��ġ�� �÷��̾�� �����ϰ� ���� (Trigger ������Ʈ�� �浹�� �� �ֵ���)
        transform.position = player.transform.position;

        if (isTrigger == true)
        {
            controller.SetNextTutorial();
        }
    }

    public override void Exit()
    {
        //Trigger ������Ʈ ��Ȱ��ȭ
        triggerObject.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Define.Tag.TutorialTrigger.ToString()))
        {
            isTrigger = true;

            collision.gameObject.SetActive(false);
        }
    }
}
