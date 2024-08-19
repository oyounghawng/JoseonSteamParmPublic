using System.Collections;
using UnityEngine;

public class PlayerAction : MonoBehaviour
{
    private PlayerInputController playerInputController;
    private PlayerMovement playerMovement;
    private Player player;

    private float rayDistance = 1f;
    private bool isAction = false;
    public LayerMask layerMask;


    private void Awake()
    {
        playerInputController = GetComponent<PlayerInputController>();
        playerMovement = GetComponent<PlayerMovement>();
        player = GetComponent<Player>();
    }

    private void Start()
    {
        playerInputController.OnActionEvent += Action;
    }


    private void Action()
    {
        if (isAction)
            return;
        isAction = true;
        Vector2 direcion = playerMovement.LookDir;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direcion, rayDistance, layerMask);
        if (player.curEquip == null)
        {
            //TODO : Animal
            //Animal animal;
            //if (hit.transform.gameObject.TryGetComponent(out animal))
            //{
            //    animal.Affection = 10;
            //    StartCoroutine(OnAction());
            //    return;
            //}
        }
        else
        {
            if (player.curEquip.TryGetComponent(out ITool itool))
            {
                if (player.curEquip.TryGetComponent(out Tool tool))
                {
                    playerInputController.isAction = true;
                    (UIManager.Instance.SceneUI as UI_GameScene).isActive = false;
                    itool.Subscribe(playerInputController.OnBehavior);
                    itool.Subscribe(player.animationController.EndAction);
                    itool.UseAnimation(player.animationController);
                }
                itool.Use();
            }
        }
        StartCoroutine(OnAction());
    }
    IEnumerator OnAction()
    {
        yield return new WaitForSeconds(0.5f);
        isAction = false;
    }
}
