using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_SellSlot : UI_ShopSlot
{
    public Action OnItemSold;

    public override void Init()
    {
        base.Init();
        priceText.text = data.sellPrice.ToString();
    }
    protected override void OnClickEvent(PointerEventData eventData)
    {
        //있는지를 탐색
        //GameManagerEx.Instance.SaveData.Gold = data.sellPrice;
        (SceneManagerEx.Instance.CurrentScene as GameScene).player.RemoveItem(itemOjbect.itemObjectData);

        OnItemSold?.Invoke();
    }
}
