using TMPro;

public class UI_ItemToolTip : UI_Base
{
    public static UI_ItemToolTip instance;

    ItemObjectData itemData;

    enum Texts
    {
        InfoText
    }

    void Start()
    {
        Init();
    }
    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));

        instance = this;
        GetText((int)Texts.InfoText).text = string.Empty;
        this.gameObject.SetActive(false);
    }

    public void ShowToolTip(ItemObjectData itemData)
    {
        this.gameObject.SetActive(true);
        UpdateText(itemData.GetTooltip());
    }

    public void HideToolTip()
    {
        this.gameObject.SetActive(false);
    }

    private void UpdateText(string tooltipString)
    {
        GetText((int)Texts.InfoText).text = tooltipString;

        //[농사템 계절, 자라는시간] , 낚시템 []
    }
}
