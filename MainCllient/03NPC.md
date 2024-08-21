# JoseonSteamParmPublic
조선스팀팜 코드공개용 레퍼지터리

- NPC
    
    <aside>
    💡 상속을 통해 일반  NPC와 주 NPC의 기능 분리
    State 패턴을 구현하여 상태 별 NPC 패턴 구현
    
    - NPC가 가져야할 기본 변수와 Component들을 상속할 클래스에 미리 구현하여 유지보수성을 확장
    - 상태 패턴을 활용하여 특정 상황에 불필요한 행동을 방지하고 보다 더 구체적으로 기능을 구현하고 확장 가능
    
    ![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/dc764bea-8e0a-428e-8e44-b767a3c218f9/image.png)
    
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
    
    ![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/5aa2c9d0-5e6e-41a8-af1f-373cac0cbee4/image.png)
    
    ![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/65c69283-ffc9-443b-800e-86921753f0e3/image.png)
    
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
    
    ![image.png](https://prod-files-secure.s3.us-west-2.amazonaws.com/83c75a39-3aba-4ba4-a792-7aefe4b07895/d1dcf920-b36c-4401-bcf5-298408fbf41b/image.png)
    