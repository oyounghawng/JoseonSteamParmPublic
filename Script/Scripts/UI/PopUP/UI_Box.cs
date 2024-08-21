using UnityEngine;

public class UI_Box : UI_Popup
{
    private ItemObjectData[] playerInventory;
    private ItemObjectData[] boxInventory;

    private GameObject boxInventoryPanel;
    private GameObject playerinventoryPanel;

    enum GameObjects
    {
        BoxInventorySlotPanel,
        PlayerInventorySlotPanel,
    }

    void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        boxInventoryPanel = Get<GameObject>((int)GameObjects.BoxInventorySlotPanel);
        playerinventoryPanel = Get<GameObject>((int)GameObjects.PlayerInventorySlotPanel);

        playerInventory = (SceneManagerEx.Instance.CurrentScene as GameScene).player.playerInventory;
        SetBoxInven();
        SetPlayerInven();
    }

    void SetBoxInven()
    {
        foreach (Transform child in boxInventoryPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        //사용가능 인벤토리 + 첫줄은 퀵슬릇 창이랑 동기화
        for (int i = 0; i < boxInventory.Length; i++)
        {
            UI_ItemSlot ItemSlot = Managers.UI.MakeSubItem<UI_ItemSlot>(boxInventoryPanel.transform);
            ItemSlot.SetIndex(i, true, boxInventory);
        }
    }
    void SetPlayerInven()
    {
        foreach (Transform child in playerinventoryPanel.transform)
            Managers.Resource.Destroy(child.gameObject);

        int bagLevel = (Managers.Scene.CurrentScene as GameScene).player.PlayerInventoryLevel;
        bagLevel = (bagLevel + 1) * 10;

        //사용가능 인벤토리
        for (int i = 0; i < bagLevel; i++)
        {
            UI_ItemSlot ItemSlot = Managers.UI.MakeSubItem<UI_ItemSlot>(playerinventoryPanel.transform);
            ItemSlot.SetIndex(i, true, playerInventory);
        }

        //사용 불가 인벤토리(확장필요)
        for (int i = bagLevel + 1; i < 31; i++)
        {
            UI_ItemSlot ItemSlot = Managers.UI.MakeSubItem<UI_ItemSlot>(playerinventoryPanel.transform);
            ItemSlot.SetIndex(i, false);
        }
    }

    public void SetBoxItemList(ItemObjectData[] _boxInventory)
    {
        boxInventory = _boxInventory;
    }
}
