using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInputController : InputController
{
    public void OnMove(InputAction.CallbackContext context)
    { 
        if (UIManager.Instance.popupCount != 0 || isMovementRestricted)
        {
            CallMoveEvent(Vector2.zero);
            return;
        }

        Vector2 moveInput = context.ReadValue<Vector2>();
        CallMoveEvent(moveInput);
    }

    public void OnAction(InputAction.CallbackContext context)
    {
        if (isMovementRestricted)
            return;

        if (UIManager.Instance.popupCount != 0)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (context.phase == InputActionPhase.Started)
        {
            CallActionEventt();
        }
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (isMovementRestricted)
            return;

        if (UIManager.Instance.popupCount != 0)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (context.phase == InputActionPhase.Started)
        {
            CallInteractionEvent();
        }
    }

    public async void OnInventory(InputAction.CallbackContext context)
    {
        if (isMovementRestricted)
            return;

        if (context.phase == InputActionPhase.Started)
        {
            if (Managers.UI.FindPopup<UI_Inventory>())
            {
                Time.timeScale = 1f;
                UIManager.Instance.ClosePopupUI();
            }
            else
            {
                if (Managers.UI.popupCount > 0)
                    return;

                Time.timeScale = 0f;
                await Managers.UI.ShowTaskPopupUI<UI_Inventory>();
            }
        }
    }

    public void OnBehavior(bool action) => isAction = action;
    public bool isMovementRestricted = false;
}