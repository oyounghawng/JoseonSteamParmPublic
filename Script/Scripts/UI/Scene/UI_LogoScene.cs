using UnityEngine;
using UnityEngine.UI;

public class UI_LogoScene : UI_Scene
{
    public Button btn_Start;
    public Button btn_Load;
    public Button btn_Setting;
    public Button btn_Developer;
    public Button btn_Quit;


    void Start()
    {
        Init();
    }
    public override void Init()
    {
        base.Init();

        btn_Start.onClick.AddListener(ActiveStart);

        btn_Load.onClick.AddListener(ActiveLoad);

        btn_Setting.onClick.AddListener(ActiveSetting);

        btn_Developer.onClick.AddListener(ActiveDeveloper);

        btn_Quit.onClick.AddListener(ActiveQuit);

    }

    private async void ActiveStart()
    {
        await Managers.UI.ShowTaskPopupUI<UI_CharacterCustom>();

    }

    private async void ActiveLoad()
    {
        
    }

    private async void ActiveSetting()
    {
        await Managers.UI.ShowTaskPopupUI<UI_Setting>();
    }

    private async void ActiveDeveloper()
    {
        await Managers.UI.ShowTaskPopupUI<UI_Developer>();

    }

    private void ActiveQuit()
    {
        Application.Quit();
    }
}
