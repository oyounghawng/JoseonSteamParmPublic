# NPC    
-
    <aside>
    💡 상속을 통해 일반  NPC와 주 NPC의 기능 분리
    State 패턴을 구현하여 상태 별 NPC 패턴 구현
    
    - NPC가 가져야할 기본 변수와 Component들을 상속할 클래스에 미리 구현하여 유지보수성을 확장
    - 상태 패턴을 활용하여 특정 상황에 불필요한 행동을 방지하고 보다 더 구체적으로 기능을 구현하고 확장 가능
    
    ![image](https://github.com/user-attachments/assets/5bc5b55a-0cc5-4af8-9b38-0af56b3e86f0)

    - 코드
        
        ```csharp
        
        public abstract class NPC : MonoBehaviour, IInteractable
        {
        
        		...
        
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
        
            protected readonly int hashIdle = Animator.StringToHash("isIdle");
            protected readonly int hashMove = Animator.StringToHash("isWalk");
            protected readonly int hashAction = Animator.StringToHash("isAction");
        
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
        ```
        
    
    <aside>
    💡 NPC 관련 데이터를 구글 스프레드시트에 저장 및 관리하여, 접근성 향상
    
    ‼️Routine은 일자 별 시작 시간, 끝 시간, 목적지, 최종 바라보는 방향, 행동을 저장
    
    ![image](https://github.com/user-attachments/assets/24c2e7b1-110f-4c1c-83ac-82177f4deddf)

    ![image](https://github.com/user-attachments/assets/988a15e0-b95d-4853-bdae-6cc4ce47030c)

    - 코드
        
        ```csharp
            private void GetDayRoutine(int day)
            {
                if (routines.Count > 0)
                {
                    dayRoutine = routines.FindAll(x => x.dayofweek == TimeManager.Instance.Day % 7);
                }
            }
            private async void DoRoutine(int time)
            {
                while (dayRoutine.Count < 0 && dayRoutine == null) await Task.Yield();
                if (dayRoutine != null && routines.Count > 0 && routines != null)
                {
                    foreach (var rt in dayRoutine)
                    {
                        if (rt.endTime >= time)
                        {
                            if (CurrentRoutine != rt)
                                CurrentRoutine = rt;
                            break;
                        }
                    }
                }
            }
        ```
        
    </aside>

    </aside>
