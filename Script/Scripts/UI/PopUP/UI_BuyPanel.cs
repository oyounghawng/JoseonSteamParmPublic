using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UI_BuyPanel : UI_Base
{
    [SerializeField] List<ItemData> buyDatas;

    enum GameObjects
    {
        BuySlotPanel
    }
    void Start()
    {
        Init();
    }

    public override void Init()
    {
        Bind<GameObject>(typeof(GameObjects));

        //TODO¾¾¾Ñ ÆÄ´Â°Å °èÀýÃß°¡
        buyDatas = DataManager.Instance.ItemDatas.Values.
            Where(data => data.type == 1 && data.displayName.Contains("¾¾¾Ñ")
            && Managers.Data.FarmDatas[data.relationData].Season == Enum.GetName(typeof(Define.Season), TimeManager.Instance.Season)).ToList();
        Set();
    }
    private void Set()
    {
        GameObject go = GetObject((int)GameObjects.BuySlotPanel);
        foreach (Transform child in go.transform)
            Managers.Resource.Destroy(child.gameObject);

        for (int i = 0; i < buyDatas.Count; i++)
        {
            UI_BuySlot buySlot = Managers.UI.MakeSubItem<UI_BuySlot>(go.transform);
            buySlot.SetData(buyDatas[i]);
        }
    }
}
