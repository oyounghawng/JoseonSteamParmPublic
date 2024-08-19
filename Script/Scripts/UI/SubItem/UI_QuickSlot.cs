using TMPro;
public class UI_QuickSlot : UI_ItemSlot
{
    public TextMeshProUGUI numtext;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        numtext.text = index == 9 ? "0" : (index + 1).ToString();
    }

    protected override void ChangeSlot()
    {
        base.ChangeSlot();
    }
}
