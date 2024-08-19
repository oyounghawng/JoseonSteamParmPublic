using UnityEngine;

public class PickAxe : Tool, ITool
{
    public bool Use()
    {
        Vector3 position = transform.position + lookDir;
        int resourcesLayerMask = 1 << (int)Define.LayerMask.Resources;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDir, 1f, resourcesLayerMask);
        if (hit)
        {
            hit.transform.TryGetComponent(out ResourcesItem resource);
            if (resource.type == ResourceType.Stone)
            {
                resource.ItemDrop();
            }
        }
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
