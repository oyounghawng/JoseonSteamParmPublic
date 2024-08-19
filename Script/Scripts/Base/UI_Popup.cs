using UnityEngine;

public class UI_Popup : UI_Base
{
    public override void Init()
    {
        Managers.UI.SetCanvas(gameObject, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ClosePopupUI();
            Time.timeScale = 1f;
        }
    }

    public virtual void ClosePopupUI()
    {
        Managers.UI.ClosePopupUI(this);
    }
}
