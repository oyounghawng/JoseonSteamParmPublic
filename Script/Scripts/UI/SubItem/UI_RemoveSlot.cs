using UnityEngine.EventSystems;

public class UI_RemoveSlot : UI_Base, IDropHandler
{
    private UI_DragSlot instance;
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        instance = UI_DragSlot.instance;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (instance.itemSlot.slotItemData.itemData.type == (int)Define.ItemType.ToolItem)
            return;

        (Managers.Scene.CurrentScene as GameScene).player.RemoveItem(instance.itemSlot.slotItemData);
    }
}
