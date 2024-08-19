using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_VillageNPCManagement : UI_Popup
{
    public TMP_Text txtTitle;

    public Transform npcInfoRoot;

    public override void Init()
    {
        base.Init();
        CreateNPCInfoSlot();
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }

    private void CreateNPCInfoSlot()
    {
        List<NPCData> npcLIst = Managers.Game.SaveData.villageData.npcData;

        for(int i = 0; i < npcLIst.Count; i++)
        {
            UI_NPCInfoElement element = Managers.UI.MakeSubItem<UI_NPCInfoElement>(npcInfoRoot);

            element.Init(npcLIst[i]);
        }

    }
}