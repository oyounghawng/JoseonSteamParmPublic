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
        (SceneManagerEx.Instance.CurrentScene as GameScene).player.RemoveItem(itemOjbect.itemObjectData);

        Managers.Game.SaveData.Gold += itemOjbect.itemObjectData.itemData.sellPrice;
        OnItemSold?.Invoke();
    }
}
