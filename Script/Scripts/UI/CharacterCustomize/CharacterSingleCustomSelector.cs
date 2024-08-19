using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 종류 또는 속성만을 변경하는 선택자
/// </summary>
public class CharacterSingleCustomSelector : CharacterCustomSelector
{
    [field: SerializeField]
    TMP_Text txtCounter = null;

    [field: SerializeField]
    Button prevButton = null;

    [field: SerializeField]
    Button nextButton = null;

    [Range(-1, 100)]
    [SerializeField]
    private int selCount = 0;

    public int SelCount
    {
        get => selCount;
        set
        {
            selCount = value;
            txtCounter.text = $"{selCount}";
            nextButton.interactable = (assets.Count - 1 > selCount);
            prevButton.interactable = (selCount > 0);
            SwapParts(assets[selCount]);
        }
    }

    protected override void Init()
    {
        base.Init();
        SelCount = 0;
        ActiveResolver();
    }

    // Top, Under, Hair -> 카테고리
    // 카테고리 -> SpriteLibraryAssetw

    protected override void OnEnable()
    {
        base.OnEnable();
        nextButton.onClick.AddListener(NextParts);
        prevButton.onClick.AddListener(PrevParts);

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        nextButton.onClick.RemoveListener(NextParts);
        prevButton.onClick.RemoveListener(PrevParts);

    }
    protected override void NextParts()
    {
        base.NextParts();
        SelCount = assets.Count - 1 > SelCount ? ++SelCount : SelCount;
    }

    protected override void PrevParts()
    {
        base.PrevParts();
        SelCount = SelCount > -1 ? --SelCount : SelCount;
    }

    private void ActiveResolver()
    {
        switch (parts)
        {
            case Define.CharacterParts.Hat:
                toggle.isOn = false;
                toggle.interactable = true;
                charResolver.gameObject.SetActive(false);
                break;

            case Define.CharacterParts.Blush:
                toggle.isOn = false;
                toggle.interactable = true;
                charResolver.gameObject.SetActive(false);
                break;

            case Define.CharacterParts.Accessory1:
                toggle.isOn = false;
                toggle.interactable = true;
                charResolver.gameObject.SetActive(false);
                break;

            case Define.CharacterParts.Eye:
                toggle.interactable = false;
                break;

            case Define.CharacterParts.Lipstick:
                toggle.interactable = false;
                break;

            case Define.CharacterParts.Body:
                toggle.interactable = false;
                break;
        }
    }

}