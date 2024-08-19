using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Seeding : MonoBehaviour, ITool
{
    public ItemObjectData itemObjectData;
    public void SetItemObjectData(ItemObjectData _itemObjectData)
    {
        itemObjectData = _itemObjectData;
    }
    public bool Use()
    {
        //TODO 건축이 가능한 위치 설정
        Vector3 position = (SceneManagerEx.Instance.CurrentScene as GameScene).player.transform.position;
        Vector3Int gridPos = GridManager.Instance.interactableMap.WorldToCell(position);
        TileBase clickedTile = GridManager.Instance.interactableMap.GetTile(gridPos);
        if (clickedTile == GridManager.Instance.plowedTile || clickedTile == GridManager.Instance.wetTile)
        {
            GridManager.Instance.Plant(gridPos, itemObjectData);
        }
        return true;
    }
    public void Subscribe(Action<bool> callback)
    {
        
    }

    public void UseAnimation(PlayerAnimationController animationController)
    {
        
    }

}
