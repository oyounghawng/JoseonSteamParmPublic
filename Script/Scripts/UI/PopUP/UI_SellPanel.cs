using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_SellPanel : UI_Base
{
    [SerializeField] List<ItemObjectData> sellDatas = new List<ItemObjectData>();
    [SerializeField] List<UI_SellSlot> sellSlots = new List<UI_SellSlot>();
    private bool isBind = false;
    enum GameObjects
    {
        SellSlotPanel
    }
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));
        UpDateSellList();
        isBind = true;
        //sellDatas = DataManager.Instance.ItemDatas.Values.Where(data => data.type == 1 && !data.displayName.Contains("¾¾¾Ñ")).ToList();
    }

    private void OnEnable()
    {
        if (isBind)
            UpDateSellList();
    }
    private void UpDateSellList()
    {
        sellDatas = (SceneManagerEx.Instance.CurrentScene as GameScene).player.playerInventory.Where(data => data != null && data.itemData.type < 4).ToList();
        Set();
    }
    private void Set()
    {
        GameObject go = GetObject((int)GameObjects.SellSlotPanel);
        foreach (Transform child in go.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < sellDatas.Count; i++)
        {
            UI_SellSlot buySlot = Managers.UI.MakeSubItem<UI_SellSlot>(go.transform);
            sellSlots.Add(buySlot);
            buySlot.SetData(sellDatas[i].itemData);
            buySlot.OnItemSold += UpDateSellList;
        }
    }

}
