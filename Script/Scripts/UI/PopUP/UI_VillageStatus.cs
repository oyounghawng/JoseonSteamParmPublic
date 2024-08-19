using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_VillageStatus : UI_Popup
{
    public List<RequiredMonthlyItemDataSO> requiredMonthlyItemData;
    List<UI_RequireItemSlot> slots = new List<UI_RequireItemSlot>();

    [SerializeField]
    private VillageData data = null;
    public VillageData Data
    {
        get
        {
            return data;
        }
        set
        {
            data = value;
            SetTitle(Enum.GetName(typeof(Define.Season), TimeManager.Instance.Season));
            SetDDay(28 - TimeManager.Instance.Day);
            SetReputation(Data.reputation);
            SetPopulation(Data.population, Data.maxPopulation);
            CreateRequireSlot();
        }
    }


    #region UI Elements

    public GameObject content;

    public GameObject dDayObj;

    public GameObject requiredItemObj;

    public Transform requiredItemRoot;

    public GameObject villageReputationObj;

    public GameObject villagePopulationObj;

    public TMP_Text txt_Title;

    public TMP_Text txt_DDay;

    public TMP_Text txt_Repulation;

    public TMP_Text txt_Population;

    #endregion

    public override void Init()
    {
        base.Init();
        Data = Managers.Game.SaveData.villageData;
    }


    private void CreateRequireSlot()
    {
        if (slots.Count > 0)
        {
            foreach (var slot in slots)
            {
                Destroy(slot.gameObject);
            }
            slots.Clear();
        }

        // 계절 별 요구 사항
        foreach (var required in requiredMonthlyItemData)
        {
            if (required != null)
            {
                // 현재 계졀의 요구 사항 발견 시
                if (required.season == TimeManager.Instance.Season)
                {
                    foreach (var item in required.requiredItemDatas)
                    {
                        UI_RequireItemSlot slot = Managers.UI.MakeSubItem<UI_RequireItemSlot>(requiredItemRoot);
                        slot.RequireData = item;
                        slots.Add(slot);
                    }
                    break;
                }
            }
        }
    }

    private void SetTitle(string season)
    {
        txt_Title.text = $"{season} 세금 징수서";
    }
    private void SetDDay(int dday)
    {
        txt_DDay.text = $"세금 징수까지 앞으로 D-{dday}";
    }

    private void SetReputation(int reputation)
    {
        txt_Repulation.text = $"{reputation}";
    }
    private void SetPopulation(int population, int maxPopulation)
    {
        txt_Population.text = $"{population} / {maxPopulation}";
    }
}
