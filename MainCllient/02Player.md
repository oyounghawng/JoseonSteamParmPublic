# Player    
-
    <aside>
    💡 **플레이어의 기능을 클래스로 세분화**
    
    ---
    
    📝 **플레이어의 각 기능을 세분화하여 하나의 클래스에 복잡한 코드를 방지합니다.**
    
    - 오류 및 버그를 찾기 쉬워지면서 유지보수성을 높아집니다.
    - 기능별로 나누어져있기에 가독성이 높아집니다.
    - Fan-in을 높이고 Fan-out을 낮추어 시스템의 복잡성을 최적화합니다.
    
    ![image](https://github.com/user-attachments/assets/da6f55a6-9bf5-4e19-9f16-9cdb3f3216f6)

    **📝대리자 Action과 Input System을 활용한 옵저버 패턴 활용**
    
    ---
    
    - PlayerMovement, PlayerAnimaion, PlayerAction 등이 이를 직접 참조하여 Event에 기능을 구독함으로써 클래스 간 **결합도가 감소하며, 코드 가독성이 향상합니다.**
    - 코드
        
        ```csharp
        public class InputController : MonoBehaviour
        {
            public event Action<Vector2> OnMoveEvent;
        
            public void CallMoveEvent(Vector2 direction)
            {
                OnMoveEvent?.Invoke(direction);
            }
        
        }
        ```
        
        ```csharp
        public class PlayerInputController : InputController
        {
            public void OnMove(InputAction.CallbackContext context)
            {
                if (UIManager.Instance.popupCount != 0)
                {
                    CallMoveEvent(Vector2.zero);
                    return;
                }
        
                Vector2 moveInput = context.ReadValue<Vector2>();
                CallMoveEvent(moveInput);
            }
        }
        ```
        
        ```csharp
        public class PlayerMovement : MonoBehaviour
        {
            private void Start()
            {
                playerInputController.OnMoveEvent += Move;
            }
        }
        ```
        
    
    **📝 Action 또는 Interaction에서는 ITool, IInteractable 인터페이스를 상속한 오브젝트와 연계하여 코드를 간결화하고  합니다.**
    
    - 이로 인해 객체지향의 **의존성 역전 원칙**을 갖게 됩니다.
    - 보다 유연하고 확장성 있게 다형성을 구현해낼 수 있습니다.
    
    ![image](https://github.com/user-attachments/assets/f0c0cb7f-8062-417f-a55a-7b050dd04bc1)

    - 코드
        
        ```csharp
        public class PlayerAction : MonoBehaviour
        {
        		...
        		
        		if (player.curEquip.TryGetComponent(out ITool itool))
        		{
        				... 
        				
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
        		
        		...
        }
        ```
        
        ```csharp
        ```csharp
        
        public class PlayerInteraction : MonoBehaviour
        {
        		...
        		
            private void Interaction()
            {
                if (isInteraction)
                    return;
        
                isInteraction = true;
                Vector2 dir = playerMovement.LookDir;
                RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, rayDistance, layerMask);
                if (hit.collider != null)
                {
                    if (hit.collider.gameObject.TryGetComponent(out curInteractable))
                    {
                        curInteractable?.Interact();
                    }
                }
                
                ...
            }
            
        		```
        }
        ```
        
    </aside>
    
