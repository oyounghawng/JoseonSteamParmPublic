using UnityEngine;
using UnityEngine.Tilemaps;

public class Hoe : Tool, ITool
{
    public bool Use()
    {
        Vector3 position = transform.position + lookDir;
        Vector3Int gridPos = GridManager.Instance.interactableMap.WorldToCell(position);
        TileBase clickedTile = GridManager.Instance.interactableMap.GetTile(gridPos);

        if (clickedTile == GridManager.Instance.hiddenInteractableTile)
        {
            GridManager.Instance.PlowedTile(gridPos);
            Managers.Sound.Play(Define.Sound.Effect, "Effect/Farm_plough", Managers.Sound.EffectVolume);
        }

        if (clickedTile == GridManager.Instance.wetTile
            || clickedTile == GridManager.Instance.plowedTile)
        {
            GridManager.Instance.Havest(gridPos);
            Managers.Sound.Play(Define.Sound.Effect, "Effect/Farm_havest", Managers.Sound.EffectVolume);
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
