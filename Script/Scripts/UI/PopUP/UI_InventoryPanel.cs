using System.Collections.Generic;
using UnityEngine;

public class UI_InventoryPanel : UI_Base
{
    private GameObject inventorySlotPanel;
    private ItemObjectData[] playerInventory;
    List<UI_ItemSlot> slots;

    enum GameObjects
    {
        InventorySlotPanel,
    }
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        slots = new List<UI_ItemSlot>();
        inventorySlotPanel = GetObject((int)GameObjects.InventorySlotPanel);
        playerInventory = (SceneManagerEx.Instance.CurrentScene as GameScene).player.playerInventory;
        (Managers.Scene.CurrentScene as GameScene).player.resetSlotsUI += ResetInvenSlot;

        SetInven();

    }
    private void OnDestroy()
    {
        (Managers.Scene.CurrentScene as GameScene).player.resetSlotsUI -= ResetInvenSlot;
    }
    void SetInven()
    {
        foreach (Transform child in inventorySlotPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        int bagLevel = (Managers.Scene.CurrentScene as GameScene).player.PlayerInventoryLevel;
        bagLevel = (bagLevel + 1) * 10;

        //사용가능 인벤토리 + 첫줄은 퀵슬릇 창이랑 동기화
        for (int i = 0; i < 10; i++)
        {
            UI_QuickSlot QuickSlot = Managers.UI.MakeSubItem<UI_QuickSlot>(inventorySlotPanel.transform);
            QuickSlot.SetIndex(i, true, playerInventory);
            slots.Add(QuickSlot);
        }

        //사용가능 인벤토리
        for (int i = 10; i < bagLevel; i++)
        {
            UI_ItemSlot ItemSlot = Managers.UI.MakeSubItem<UI_ItemSlot>(inventorySlotPanel.transform);
            ItemSlot.SetIndex(i, true, playerInventory);
        }

        //사용 불가 인벤토리(확장필요)
        for (int i = bagLevel + 1; i < 31; i++)
        {
            UI_ItemSlot ItemSlot = Managers.UI.MakeSubItem<UI_ItemSlot>(inventorySlotPanel.transform);
            ItemSlot.SetIndex(i, false);
        }
    }
    private void ResetInvenSlot()
    {
        for (int i = 0; i < 10; i++)
        {
            slots[i].Set();
        }
    }
}
