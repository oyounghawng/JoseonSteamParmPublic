using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Tool : MonoBehaviour
{
    public Player player;

    public GameObject guideLine;

    public Action<bool> onEndAnimation;
    public Define.Tool toolType;

    [field: SerializeField] protected Animator animator;
    protected PlayerAnimationController controller;
    private Tilemap interactedMap;
    protected Vector3 lookDir = Vector3.zero;
    private Vector3 guidePos = Vector3.zero;

    protected readonly int hashAction = Animator.StringToHash("Action");
    protected readonly int hashXVelocity = Animator.StringToHash("xVelocity");
    protected readonly int hashYVelocity = Animator.StringToHash("yVelocity");

    private void Start()
    {
        player = (Managers.Scene.CurrentScene as GameScene).player;
    }
    private void OnEnable()
    {
        if (toolType != Define.Tool.Rod)
            guideLine.SetActive(true);
    }

    private void OnDisable()
    {
        if (toolType != Define.Tool.Rod)
            guideLine.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (guideLine.activeInHierarchy)
        {
            guidePos = transform.position + new Vector3(player.animationController.GetXVelocity(), player.animationController.GetYVelocity(), 0);
            guideLine.transform.position = GridManager.Instance.WorldToCell(guidePos, Define.TilemapType.InteractableMap) + new Vector3(0.5f, 0.5f, 0);
        }

    }
    public virtual void UseAnimation(PlayerAnimationController animationController)
    {
        controller = animationController;
        float x = 0;
        float y = 0;

        if (animationController != null)
        {
            x = animationController.GetXVelocity();
            y = animationController.GetYVelocity();

            animationController.ActionTool(toolType);
        }
        lookDir = new Vector3(x, y, 0);
        lookDir.y = lookDir.y < 0 ? lookDir.y + 0.5f : lookDir.y;

        animator.SetFloat(hashXVelocity, x);
        animator.SetFloat(hashYVelocity, y);
    }


    public virtual void Subscribe(Action<bool> callback)
    {
        onEndAnimation += callback;
    }

}
