using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Shop : UI_Popup
{
    //TODO : 스위치 패널이 많아지면 이런식으로하면 힘들듯 새롭게 고민해보기
    private GameObject buyPanel;
    private GameObject sellPanel;
    enum GameObjects
    {
        UI_BuyPanel,
        UI_SellPanel,
    }
    enum Buttons
    {
        BuyBtn,
        SellBtn,
    }
    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<GameObject>(typeof(GameObjects));
        Bind<Button>(typeof(Buttons));

        buyPanel = GetObject((int)GameObjects.UI_BuyPanel);
        sellPanel = GetObject((int)GameObjects.UI_SellPanel);

        GetButton((int)Buttons.BuyBtn).gameObject.BindEvent(OpenBuyPanel);
        GetButton((int)Buttons.SellBtn).gameObject.BindEvent(OpenSellPanel);

        sellPanel.SetActive(false);
    }

    void OpenBuyPanel(PointerEventData evt)
    {
        buyPanel.SetActive(true);
        sellPanel.SetActive(false);
    }

    void OpenSellPanel(PointerEventData evt)
    {
        buyPanel.SetActive(false);
        sellPanel.SetActive(true);
    }
}
