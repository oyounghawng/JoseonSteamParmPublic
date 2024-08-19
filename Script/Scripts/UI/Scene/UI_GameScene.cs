using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_GameScene : UI_Scene
{
    public UI_QuickSlot[] slots;
    private UI_QuickSlot currentslot;

    #region Time
    public bool isActive = true;

    enum GameObjects
    {
        QuickSlotPanel,
    }
    enum Buttons
    {
        OpenShop
    }

    private Player player;
    public TMP_Text txt_time;
    public TMP_Text txt_Day;
    public TMP_Text txt_Season;
    public TMP_Text txt_Weather;
    public TMP_Text txt_Gold;

    private Coroutine coEndFade = null;
    #endregion

    void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();
        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        player = (SceneManagerEx.Instance.CurrentScene as GameScene).player;

        player.resetPlayerEquipItem += ResetQuickSlotActive;
        player.resetSlotsUI += ResetQuickSlotUI;

        GetButton((int)Buttons.OpenShop).gameObject.BindEvent(OpenShop);
        SetQuickSlot();
        ResetUI();
    }

    private void NextDay(PointerEventData evt)
    {
        TimeManager.Instance.EndDay();
    }
    private async void OpenShop(PointerEventData evt)
    {
        await UIManager.Instance.ShowTaskPopupUI<UI_Shop>();
    }

    private void OnEnable()
    {
        TimeManager.Instance.onChangedTime -= SetTime;
        TimeManager.Instance.onChangedDay -= SetDay;
        TimeManager.Instance.onChangedSeason -= SetSeason;
        TimeManager.Instance.onChangedWeather -= SetWeather;

        TimeManager.Instance.onChangedTime += SetTime;
        TimeManager.Instance.onChangedDay += SetDay;
        TimeManager.Instance.onChangedSeason += SetSeason;
        TimeManager.Instance.onChangedWeather += SetWeather;
    }


    private void Update()
    {
        if (!isActive)
            return;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            QuilckSlotActive(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            QuilckSlotActive(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            QuilckSlotActive(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            QuilckSlotActive(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            QuilckSlotActive(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            QuilckSlotActive(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            QuilckSlotActive(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            QuilckSlotActive(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            QuilckSlotActive(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            QuilckSlotActive(9);
        }
    }
    private void SetQuickSlot()
    {
        GameObject go = GetObject((int)GameObjects.QuickSlotPanel);
        foreach (Transform child in go.transform)
            Managers.Resource.Destroy(child.gameObject);
        for (int i = 0; i < 10; i++)
        {
            UI_QuickSlot QuickSlot = Managers.UI.MakeSubItem<UI_QuickSlot>(go.transform);
            QuickSlot.SetIndex(i, true, player.playerInventory);
            slots[i] = QuickSlot;
        }
    }

    private void ResetUI()
    {
        txt_Gold.text = Managers.Game.SaveData.Gold.ToString();
    }

    private void ResetQuickSlotUI()
    {
        for (int i = 0; i < 10; i++)
        {
            slots[i].Set();
        }
    }
    private void ResetQuickSlotActive()
    {
        if (currentslot != null)
        {
            currentslot.GetComponentInChildren<Outline>().enabled = false;
            currentslot = null;
        }
    }

    private void QuilckSlotActive(int index)
    {
        if (!player.SwitchEquip(index))
            return;

        if (currentslot == null)
        {
            currentslot = slots[index];
            currentslot.GetComponentInChildren<Outline>().enabled = true;
        }
        else
        {
            currentslot.GetComponentInChildren<Outline>().enabled = false;
            if (currentslot == slots[index])
            {
                currentslot = null;
            }
            else
            {
                currentslot = slots[index];
                currentslot.GetComponentInChildren<Outline>().enabled = true;
            }
        }
    }

    private void SetSeason(int season)
    {
        string sSeason = Enum.GetName(typeof(Define.Season), season);
        txt_Season.text = sSeason;
    }

    private void SetDay(int day)
    {
        string sDay = Enum.GetName(typeof(Define.Day), day % 7);
        txt_Day.text = $"{sDay}. {day}";
    }
    private void SetWeather(int weather)
    {
        string sWeahter = Enum.GetName(typeof(Define.Weather), (Define.Weather)weather);
        txt_Weather.text = $"{sWeahter}";
    }

    private void SetTime(int allTime)
    {
        int hour = allTime / 60;
        int minute = allTime % (60 * hour);
        txt_time.text = $"{hour.ToString("D2")}:{minute.ToString("D2")}";
    }

}
