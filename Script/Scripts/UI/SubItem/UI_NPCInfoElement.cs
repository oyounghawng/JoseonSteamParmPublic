using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_NPCInfoElement : UI_Base
{
    public int id = 0;
    #region UI Elements

    public Slider sdCompanionship;
    public TMP_Text txtName;
    public TMP_Text txtGender;
    public TMP_Text txtRank;
    public TMP_Text txtJob;
    public TMP_Text txtFarmLevel;
    public TMP_Text txtFishingLevel;

    #endregion

    [SerializeField]
    private NPCData data;
    public NPCData Data
    {
        get => data;
        set
        {
            data = value;
            sdCompanionship.value = data.companionshipExp;
            txtName.text = data.displayName;
            txtGender.text = Enum.GetName(typeof(Define.Gender), data.gender);
            txtJob.text = Enum.GetName(typeof(Define.NPCRole), data.role);
            txtRank.text = Enum.GetName(typeof(Define.NPCRank), data.rank);
            txtFarmLevel.text = $"{data.farmingSkillLevel}";
            txtFishingLevel.text = $"{data.fishingSkillLevel}";
            profileImage.sprite = data.portraits[0];
        }
    }
    public Image profileImage;
    public override void Init()
    {
    }

    public void Init(NPCData data)
    {
        Data = data;
    }
}