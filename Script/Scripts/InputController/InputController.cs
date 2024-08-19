using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool isAction = false;

    public event Action<Vector2> OnMoveEvent;
    public event Action OnActionEvent;
    public event Action OnInteractionEvent;

    public void CallMoveEvent(Vector2 direction)
    {
        OnMoveEvent?.Invoke(direction);
    }

    public void CallActionEventt()
    {
        OnActionEvent?.Invoke();
    }

    public void CallInteractionEvent()
    {
        OnInteractionEvent?.Invoke();
    }
}
