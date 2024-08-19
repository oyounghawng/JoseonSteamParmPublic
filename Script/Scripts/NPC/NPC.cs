using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class NPC : MonoBehaviour, IInteractable
{
    [SerializeField]
    private NPCData npcData;
    public NPCData NPCData
    {
        get
        {
            if (npcData.portraits.Count <= 0 || npcData.portraits == null)
            {
                if (npcData.type == Define.NPCType.Normal)
                {
                    npcData.portraits = npcData.gender == Define.Gender.Male ?
                        Managers.Resource.LoadAll<Sprite>("Images/NPC00006/Male").ToList() :
                        Managers.Resource.LoadAll<Sprite>("Images/NPC00006/Female").ToList();
                }
            }
            return npcData;
        }
        set
        {
            npcData = value;
        }
    }

    private NPCDialogueContainer dialogueContainer;

    public NPCDialogueContainer DialogueContainer
    {
        get
        {
            if (dialogueContainer == null)
            {
                if (Managers.Data.NPCDialogueContainer.TryGetValue(gameObject.name, out NPCDialogueData value))
                {
                    dialogueContainer = value.Value;
                }
            }
            return dialogueContainer;
        }
        set
        {
            dialogueContainer = value;
        }
    }

    #region Components

    [field: SerializeField]
    protected Animator animator;

    #endregion

    #region Animation Hash

    [HideInInspector]
    public readonly int hashXVelocity = Animator.StringToHash("xVelocity");
    [HideInInspector]
    public readonly int hashYVelocity = Animator.StringToHash("yVelocity");
    [HideInInspector]
    public readonly int hashBehavior = Animator.StringToHash("Behavior");

    public readonly int hashIdle = Animator.StringToHash("isIdle");
    public readonly int hashMove = Animator.StringToHash("isWalk");
    protected readonly int hashAction = Animator.StringToHash("isAction");

    #endregion

    #region Dialogue Option

    public bool isTalk = false;
    public bool isIntroduced = true;
    public bool isNormalTalk = false;
    public bool isManageShop = false;

    #endregion

    #region Move Option

    public bool isArrived = false;
    public float distThreshold = 0.0001f;

    #endregion

    protected FiniteStateMachine finiteStateMachine;

    protected virtual void Awake()
    {
        finiteStateMachine = new FiniteStateMachine();
    }

    protected virtual IEnumerator Start()
    {
        yield return null;
    }

    protected virtual void Update()
    {
        finiteStateMachine.CurrentState?.LogicUpdate();
    }
    protected virtual void FixedUpdate()
    {
        finiteStateMachine.CurrentState?.PhysicsUpdate();
    }

    public virtual void Interact()
    {

    }

    public bool IsArrived(Vector3 destination)
    {
        if ((destination - transform.position).sqrMagnitude < distThreshold)
            return true;

        return false;
    }
}