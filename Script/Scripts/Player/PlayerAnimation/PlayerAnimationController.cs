using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public PlayerInputController inputController;

    [SerializeField]
    private Animator animator;

    Vector2 dir;

    #region State
    public PlayerAnimationStateMachine StateMachine { get; private set; }

    public PlayerIdleAnimationState IdleState { get; private set; }
    public PlayerMoveAnimationState MoveState { get; private set; }
    public PlayerActionAnimationState ActionState { get; private set; }

    #endregion

    #region Animation Hash

    private readonly int hashXVelocity = Animator.StringToHash("xVelocity");

    private readonly int hashYVelocity = Animator.StringToHash("yVelocity");

    private readonly int hashIdle = Animator.StringToHash("isIdle");
    private readonly int hashMove = Animator.StringToHash("isWalk");
    private readonly int hashAxe = Animator.StringToHash("isAxe");
    private readonly int hashFishing = Animator.StringToHash("isFishing");
    private readonly int hashHoe = Animator.StringToHash("isHoe");
    private readonly int hashPickAxe = Animator.StringToHash("isPickAxe");
    private readonly int hashSwordSwing = Animator.StringToHash("isSwordSwing");
    private readonly int hashWater = Animator.StringToHash("isWater");
    private readonly int hashAction = Animator.StringToHash("isAction");

    #endregion

    private void Awake()
    {
        IdleState = new PlayerIdleAnimationState(this, animator, hashIdle);
        MoveState = new PlayerMoveAnimationState(this, animator, hashMove);
        ActionState = new PlayerActionAnimationState(this, animator, hashAction);

        StateMachine = new PlayerAnimationStateMachine();
    }

    private void OnEnable()
    {
        inputController.OnMoveEvent -= SetDirection;
        inputController.OnMoveEvent -= Move;
        TimeManager.Instance.onEndDay -= Reset;

        inputController.OnMoveEvent += SetDirection;
        inputController.OnMoveEvent += Move;
        TimeManager.Instance.onEndDay += Reset;
    }


    private void Start()
    {
        StateMachine.Initialize(IdleState);
    }

    public float GetXVelocity() => animator.GetFloat(hashXVelocity);
    public float GetYVelocity() => animator.GetFloat(hashYVelocity);
    private void SetDirection(Vector2 dir)
    {
        if (!inputController.isAction)
        {
            if (dir != Vector2.zero)
            {
                if (dir.x != 0 && dir.y != 0)
                {
                    animator.SetFloat(hashXVelocity, Mathf.Sign(dir.x));
                    animator.SetFloat(hashYVelocity, 0);
                }
                else
                {
                    animator.SetFloat(hashXVelocity, dir.x);
                    animator.SetFloat(hashYVelocity, dir.y);
                }

            }
        }
    }

    private void Move(Vector2 dir)
    {
        this.dir = dir;
        if (dir != Vector2.zero)
        {
            StateMachine.ChangeState(MoveState);
        }
        else
        {
            StateMachine.ChangeState(IdleState);
        }
    }

    public void ActionTool(Define.Tool toolType)
    {
        StateMachine.ChangeState(ActionState);
        switch (toolType)
        {
            case Define.Tool.Axe:
                animator.SetTrigger(hashAxe);
                break;
            case Define.Tool.Hoe:
                animator.SetTrigger(hashHoe);
                break;
            case Define.Tool.WateringCan:
                animator.SetTrigger(hashWater);
                break;
            case Define.Tool.Sword:
                animator.SetTrigger(hashSwordSwing);
                break;
            case Define.Tool.Rod:
                animator.SetBool(hashFishing, true);
                break;
            case Define.Tool.PickAxe:
                animator.SetTrigger(hashPickAxe);
                break;

            default: return;
        }
    }
    public void ActionTool(Define.Tool toolType, bool active)
    {
        if (active)
            StateMachine.ChangeState(ActionState);
        else
        {
            if (dir != Vector2.zero)
            {
                StateMachine.ChangeState(MoveState);
            }
            else
            {
                StateMachine.ChangeState(IdleState);
            }
        }

        switch (toolType)
        {
            case Define.Tool.Axe:
                animator.SetTrigger(hashAxe);
                break;
            case Define.Tool.Hoe:
                animator.SetTrigger(hashHoe);
                break;
            case Define.Tool.WateringCan:
                animator.SetTrigger(hashWater);
                break;
            case Define.Tool.Sword:
                animator.SetTrigger(hashSwordSwing);
                break;
            case Define.Tool.Rod:
                animator.SetBool(hashFishing, active);
                break;
            case Define.Tool.PickAxe:
                animator.SetTrigger(hashPickAxe);
                break;

            default: return;
        }
    }
    public void WaitThrowRod() => animator.SetBool(hashFishing, false);
    public void ThrowRod()
    {
        animator.SetBool(hashFishing, true);
    }

    public void EndAction(bool active)
    {
        if (dir != Vector2.zero)
        {
            StateMachine.ChangeState(MoveState);
        }
        else
        {
            StateMachine.ChangeState(IdleState);
        }
        (UIManager.Instance.SceneUI as UI_GameScene).isActive = true;
        SetDirection(dir);
    }

    private void Reset()
    {
        ActionTool(Define.Tool.Rod, false);
        StopAllCoroutines();
    }
}