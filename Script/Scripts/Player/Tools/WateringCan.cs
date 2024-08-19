using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class WateringCan : Tool, ITool
{
    public bool Use()
    {
        Vector3 position = transform.position + lookDir;
        Vector3Int gridPos = GridManager.Instance.interactableMap.WorldToCell(position);
        TileBase clickedTile = GridManager.Instance.interactableMap.GetTile(gridPos);

        if (clickedTile == GridManager.Instance.plowedTile)
        {
            GridManager.Instance.WetTile(gridPos);
            Managers.Sound.Play(Define.Sound.Effect, "Effect/Farm_Watering", Managers.Sound.EffectVolume);
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
