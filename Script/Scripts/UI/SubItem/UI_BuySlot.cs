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
        //구매조건 처리
        //GameManagerEx.Instance.SaveData.Gold = -data.buyPrice;
        (SceneManagerEx.Instance.CurrentScene as GameScene).player.AddItem(itemOjbect.itemObjectData);
    }
}
