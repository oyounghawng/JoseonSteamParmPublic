using System;

public interface IInteractable
{
    void Interact();
}
public interface ITool
{
    bool Use();
    void UseAnimation(PlayerAnimationController animationController);
    void Subscribe(Action<bool> callback);
}
public interface IItem
{
    void Use();
}
