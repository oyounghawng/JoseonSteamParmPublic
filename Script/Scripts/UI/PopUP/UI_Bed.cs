using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Bed : UI_Popup
{
    enum Buttons
    {
        YesBtn,
        NoBtn
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        GetButton((int)Buttons.YesBtn).gameObject.BindEvent(Accept);
        GetButton((int)Buttons.NoBtn).gameObject.BindEvent(Ignore);
    }
    public void Accept(PointerEventData evt)
    {
        UIManager.Instance.ClosePopupUI();
        TimeManager.Instance.EndDay();
    }

    public void Ignore(PointerEventData evt)
    {
        UIManager.Instance.ClosePopupUI();
    }
}
