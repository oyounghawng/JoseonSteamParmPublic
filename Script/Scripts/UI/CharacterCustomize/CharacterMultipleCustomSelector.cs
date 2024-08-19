using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMultipleCustomSelector : CharacterCustomSelector
{
    private int typeCount = 0;

    public int TypeCount
    {
        get => typeCount;

        set
        {
            typeCount = value;
            txtTypeCounter.text = $"{typeCount}";
            btnTypePrev.interactable = typeCount > 0;
            btnTypeNext.interactable = assets.Count - 1 > typeCount * colorMaxCount + colorMaxCount;
            SwapParts(assets[(typeCount * colorMaxCount) + ColorCount]);
        }
    }


    private int colorCount = 0;
    public int ColorCount
    {
        get => colorCount;
        set
        {
            colorCount = value;
            txtColorCounter.text = $"{ColorCount}";
            btnPrevColor.interactable = ColorCount > 0;
            btnNextColor.interactable = colorMaxCount - 1 > ColorCount;
            SwapParts(assets[(TypeCount * colorMaxCount) + ColorCount]);

        }
    }

    [Header("종류")]

    public Button btnTypePrev;

    public Button btnTypeNext;

    public TMP_Text txtTypeCounter;

    [Header("색상")]

    public Button btnPrevColor;

    public Button btnNextColor;

    public TMP_Text txtColorCounter;

    public int colorMaxCount;

    protected override void Init()
    {
        base.Init();

        ActiveResolver();

        TypeCount = 0;
        ColorCount = 0;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        btnTypeNext.onClick.AddListener(NextTypeParts);
        btnTypePrev.onClick.AddListener(PrevTypeParts);

        btnNextColor.onClick.AddListener(NextColorParts);
        btnPrevColor.onClick.AddListener(PrevColorParts);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        btnTypeNext.onClick.RemoveListener(NextTypeParts);
        btnTypePrev.onClick.RemoveListener(PrevTypeParts);

        btnNextColor.onClick.RemoveListener(NextColorParts);
        btnPrevColor.onClick.RemoveListener(PrevColorParts);
    }

    private void NextTypeParts()
    {
        TypeCount = assets.Count - 1 > typeCount * colorMaxCount + colorMaxCount ? ++TypeCount : TypeCount;
    }

    private void PrevTypeParts()
    {
        TypeCount = typeCount > 0 ? --TypeCount : TypeCount;
    }

    private void NextColorParts()
    {
        ColorCount = colorMaxCount - 1 > colorCount ? ++ColorCount : colorCount;
    }

    private void PrevColorParts()
    {
        ColorCount = colorCount > 0 ? --ColorCount : colorCount;
    }

    private void ActiveResolver()
    {
        switch (parts)
        {
            case Define.CharacterParts.Accessory2:
                toggle.isOn = false;
                toggle.interactable = true;
                charResolver.gameObject.SetActive(false);
                break;

            default:
                toggle.isOn = true;
                toggle.interactable = false;
                break;
        }
    }

}