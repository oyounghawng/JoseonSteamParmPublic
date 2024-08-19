using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_CharacterCustom : UI_Popup
{
    public CharacterLooks Looks { get; private set; }

    public CharacterCustomSelector[] characterCustomSelectors;

    public Button btnApply;

    public Button btnReset;

    public Button btnTurnLeft;

    public Button btnTurnRight;

    public TMP_InputField inputName;

    public TMP_InputField inputFavorite;

    public TMP_InputField inputVillageName;

    public Animator animator;

    public Sprite transparnetSprite;

    private int turnDirection;

    private int hashXVelocity = Animator.StringToHash("xVelocity");
    private int hashYVelocity = Animator.StringToHash("yVelocity");

    public override void Init()
    {
        base.Init();

        characterCustomSelectors = GetComponentsInChildren<CharacterCustomSelector>();

        btnApply.onClick.AddListener(ApplyCharacter);
        btnTurnLeft.onClick.AddListener(TurnLeft);
        btnTurnRight.onClick.AddListener(TurnRight);
        btnReset.onClick.AddListener(ResetCharacter);
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();

        btnApply.onClick.RemoveListener(ApplyCharacter);
        btnTurnLeft.onClick.RemoveListener(TurnLeft);
        btnTurnRight.onClick.RemoveListener(TurnRight);
        btnReset.onClick.RemoveListener(ResetCharacter);
    }

    private async void ApplyCharacter()
    {
        await Managers.Game.Init();
        Looks = new CharacterLooks();
        foreach (var selector in characterCustomSelectors)
        {
            string assetName = selector.charResolver.spriteLibrary.spriteLibraryAsset.name;

            switch (selector.parts)
            {
                case Define.CharacterParts.Hat:
                    Looks.hatType = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Hair:
                    Looks.hairType = assetName;
                    break;
                case Define.CharacterParts.Eye:
                    Looks.eyeType = assetName;
                    break;
                case Define.CharacterParts.Blush:
                    Looks.blushType = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Lipstick:
                    Looks.lipType = assetName;
                    break;
                case Define.CharacterParts.Body:
                    Looks.bodyType = assetName;
                    break;
                case Define.CharacterParts.Top:
                    Looks.topType = assetName;
                    break;
                case Define.CharacterParts.Under:
                    Looks.underType = assetName;
                    break;
                case Define.CharacterParts.Shoes:
                    Looks.shoesType = assetName;
                    break;
                case Define.CharacterParts.Accessory1:
                    Looks.accessory1Type = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Accessory2:
                    Looks.accessory2Type = selector.toggle.isOn ? assetName : "";
                    break;
            }
        }
        Looks = new CharacterLooks();
        foreach (var selector in characterCustomSelectors)
        {
            string assetName = selector.charResolver.spriteLibrary.spriteLibraryAsset.name;

            switch (selector.parts)
            {
                case Define.CharacterParts.Hat:
                    Looks.hatType = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Hair:
                    Looks.hairType = assetName;
                    break;
                case Define.CharacterParts.Eye:
                    Looks.eyeType = assetName;
                    break;
                case Define.CharacterParts.Blush:
                    Looks.blushType = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Lipstick:
                    Looks.lipType = assetName;
                    break;
                case Define.CharacterParts.Body:
                    Looks.bodyType = assetName;
                    break;
                case Define.CharacterParts.Top:
                    Looks.topType = assetName;
                    break;
                case Define.CharacterParts.Under:
                    Looks.underType = assetName;
                    break;
                case Define.CharacterParts.Shoes:
                    Looks.shoesType = assetName;
                    break;
                case Define.CharacterParts.Accessory1:
                    Looks.accessory1Type = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Accessory2:
                    Looks.accessory2Type = selector.toggle.isOn ? assetName : "";
                    break;
            }
        }
        Looks = new CharacterLooks();
        foreach (var selector in characterCustomSelectors)
        {
            string assetName = selector.charResolver.spriteLibrary.spriteLibraryAsset.name;

            switch (selector.parts)
            {
                case Define.CharacterParts.Hat:
                    Looks.hatType = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Hair:
                    Looks.hairType = assetName;
                    break;
                case Define.CharacterParts.Eye:
                    Looks.eyeType = assetName;
                    break;
                case Define.CharacterParts.Blush:
                    Looks.blushType = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Lipstick:
                    Looks.lipType = assetName;
                    break;
                case Define.CharacterParts.Body:
                    Looks.bodyType = assetName;
                    break;
                case Define.CharacterParts.Top:
                    Looks.topType = assetName;
                    break;
                case Define.CharacterParts.Under:
                    Looks.underType = assetName;
                    break;
                case Define.CharacterParts.Shoes:
                    Looks.shoesType = assetName;
                    break;
                case Define.CharacterParts.Accessory1:
                    Looks.accessory1Type = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Accessory2:
                    Looks.accessory2Type = selector.toggle.isOn ? assetName : "";
                    break;
            }
        }
        Looks = new CharacterLooks();
        foreach (var selector in characterCustomSelectors)
        {
            string assetName = selector.charResolver.spriteLibrary.spriteLibraryAsset.name;

            switch (selector.parts)
            {
                case Define.CharacterParts.Hat:
                    Looks.hatType = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Hair:
                    Looks.hairType = assetName;
                    break;
                case Define.CharacterParts.Eye:
                    Looks.eyeType = assetName;
                    break;
                case Define.CharacterParts.Blush:
                    Looks.blushType = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Lipstick:
                    Looks.lipType = assetName;
                    break;
                case Define.CharacterParts.Body:
                    Looks.bodyType = assetName;
                    break;
                case Define.CharacterParts.Top:
                    Looks.topType = assetName;
                    break;
                case Define.CharacterParts.Under:
                    Looks.underType = assetName;
                    break;
                case Define.CharacterParts.Shoes:
                    Looks.shoesType = assetName;
                    break;
                case Define.CharacterParts.Accessory1:
                    Looks.accessory1Type = selector.toggle.isOn ? assetName : "";
                    break;
                case Define.CharacterParts.Accessory2:
                    Looks.accessory2Type = selector.toggle.isOn ? assetName : "";
                    break;
            }
        }

        if (inputName.text == string.Empty || inputFavorite.text == string.Empty || inputVillageName.text == string.Empty)
        {
            inputName.text = "박첨지";
            inputFavorite.text = "수박";
            inputVillageName.text = "유저";
        }
        Managers.Game.SaveData.SetName(inputName.text);
        Managers.Game.SaveData.SetFavoriteThing(inputFavorite.text);
        Managers.Game.SaveData.villageName = inputVillageName.text;
        Managers.Game.SaveData.SetLooks(Looks);

        Managers.UI.CloseAllPopupUI();
        await Managers.Scene.LoadSceneAsync(Define.SceneType.GameScene);
    }

    private void TurnLeft()
    {
        if (turnDirection <= 0)
        {
            turnDirection = 3;
        }
        else
        {
            turnDirection--;

        }
        TurnCharacter();
    }

    private void TurnRight()
    {
        if (turnDirection >= 3)
        {
            turnDirection = 0;
        }
        else
        {
            turnDirection++;

        }
        TurnCharacter();
    }

    private void TurnCharacter()
    {
        Define.CharacterDirection direction;
        Enum.TryParse(Enum.GetName(typeof(Define.CharacterDirection), turnDirection), out direction);
        switch (direction)
        {
            case Define.CharacterDirection.Front:
                animator.SetFloat(hashXVelocity, 0);
                animator.SetFloat(hashYVelocity, -1);
                break;
            case Define.CharacterDirection.Back:
                animator.SetFloat(hashXVelocity, 0);
                animator.SetFloat(hashYVelocity, 1);
                break;

            case Define.CharacterDirection.Left:
                animator.SetFloat(hashXVelocity, -1);
                animator.SetFloat(hashYVelocity, 0);
                break;
            case Define.CharacterDirection.Right:
                animator.SetFloat(hashXVelocity, 1);
                animator.SetFloat(hashYVelocity, 0);
                break;
        }
    }

    public void ResetCharacter()
    {
        inputName.text = "";
        inputFavorite.text = "";
        foreach (var parts in characterCustomSelectors)
        {
            parts.ResetCharacterParts();
        }
    }
}
