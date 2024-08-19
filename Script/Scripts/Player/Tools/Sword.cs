public class Sword : Tool, ITool
{
    public bool Use()
    {
        return true;
    }

    public override void UseAnimation(PlayerAnimationController animationController)
    {
        base.UseAnimation(animationController);
        animator.SetTrigger(hashAction);
    }

    public void EndAnimation()
    {
        onEndAnimation?.Invoke(false);
        onEndAnimation = null;
    }
}
