using UnityEngine;
using UnityEngine.EventSystems;

public class UI_BuySlot : UI_ShopSlot
{
    public override void Init()
    {
        base.Init();
        priceText.text = data.buyPrice.ToString();
    }
    protected override void OnClickEvent(PointerEventData eventData)
    {
        if(Managers.Game.SaveData.Gold >= itemOjbect.itemObjectData.itemData.buyPrice)
        {
            (SceneManagerEx.Instance.CurrentScene as GameScene).player.AddItem(itemOjbect.itemObjectData);
            Managers.Game.SaveData.Gold -= itemOjbect.itemObjectData.itemData.buyPrice;
        }
    }
}
