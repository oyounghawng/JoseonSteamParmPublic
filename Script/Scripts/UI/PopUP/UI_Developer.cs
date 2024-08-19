using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Developer : UI_Popup
{
    public Button CloseButton;
    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }

    public override void Init()
    {
        base.Init();
        CloseButton.onClick.AddListener(ClosePopupUI);
    }
}
