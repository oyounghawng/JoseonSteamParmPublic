using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Inventory : UI_Popup
{
    //TODO : 스위치 패널이 많아지면 이런식으로하면 힘들듯 새롭게 고민해보기
    private GameObject inventorypanel;
    private GameObject craftpanel;
    private GameObject settingPanel;

    enum GameObjects
    {
        InventoryPanel,
        CraftPanel,
        UI_Setting,
    }
    enum Buttons
    {
        InventoryBtn,
        CraftBtn,
        SettingBtn,
    }
    void Start()
    {
        Init();
    }
    
    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        inventorypanel = GetObject((int)GameObjects.InventoryPanel);
        craftpanel = GetObject((int)GameObjects.CraftPanel);
        settingPanel = GetObject((int)GameObjects.UI_Setting);
        craftpanel.SetActive(false);
        settingPanel.SetActive(false);
        GetButton((int)Buttons.InventoryBtn).gameObject.BindEvent(OpenInventoryPanel);
        GetButton((int)Buttons.CraftBtn).gameObject.BindEvent(OpenCraftPanel);
        GetButton((int)Buttons.SettingBtn).gameObject.BindEvent(OpenSettingPanel);
    }

    void OpenInventoryPanel(PointerEventData evt)
    {
        inventorypanel.SetActive(true);
        craftpanel.SetActive(false);
        settingPanel.SetActive(false);
    }

    void OpenCraftPanel(PointerEventData evt)
    {
        inventorypanel.SetActive(false);
        craftpanel.SetActive(true);
        settingPanel.SetActive(false);
    }

    void OpenSettingPanel(PointerEventData evt)
    {
        inventorypanel.SetActive(false);
        craftpanel.SetActive(false);
        settingPanel.SetActive(true);
    }
}
