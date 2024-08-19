using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class AutoStyleSetting : MonoBehaviour
{
    public CharacterLooks looks;

    [SerializeField]
    private List<CharacterParts> characterParts;

    private void Awake()
    {
        characterParts = GetComponentsInChildren<CharacterParts>().ToList();
    }
    public void Init()
    {
        looks = Managers.Game.SaveData.Look;
        AutoClothing();
    }
    private void AutoClothing()
    {
        SpriteLibraryAsset temp = null;
        foreach(var part in characterParts)
        {
            switch(part.parts)
            {
                case Define.CharacterParts.Hat:
                    temp = looks.hairType.Equals("") ? null : Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Hat/{looks.hatType}");
                    part.gameObject.SetActive(!looks.hatType.Equals(""));
                    break;
                case Define.CharacterParts.Hair:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Hair/{looks.hairType}");
                    part.gameObject.SetActive(!looks.hairType.Equals(""));
                    break;
                case Define.CharacterParts.Eye:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Eye/{looks.eyeType}");
                    part.gameObject.SetActive(!looks.eyeType.Equals(""));
                    break;
                case Define.CharacterParts.Blush:
                    temp = looks.blushType.Equals("") ? null : Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Blush/{looks.blushType}");
                    part.gameObject.SetActive(!looks.blushType.Equals("")); 
                    break;
                case Define.CharacterParts.Lipstick:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Lipstick/{looks.lipType}");
                    part.gameObject.SetActive(!looks.lipType.Equals(""));
                    break;
                case Define.CharacterParts.Body:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Body/{looks.bodyType}");
                    part.gameObject.SetActive(!looks.bodyType.Equals(""));
                    break;
                case Define.CharacterParts.Top:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Top/{looks.topType}");
                    part.gameObject.SetActive(!looks.topType.Equals(""));
                    break;
                case Define.CharacterParts.Under:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Under/{looks.underType}");
                    part.gameObject.SetActive(!looks.underType.Equals(""));
                    break;
                case Define.CharacterParts.Shoes:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Shoes/{looks.shoesType}");
                    part.gameObject.SetActive(!looks.shoesType.Equals(""));
                    break;
                case Define.CharacterParts.Accessory1:
                    temp = looks.accessory1Type.Equals("") ? null : Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Accessory1/{looks.accessory1Type}");
                    part.gameObject.SetActive(!looks.accessory1Type.Equals(""));                    
                    break;
                case Define.CharacterParts.Accessory2:
                    temp = looks.accessory2Type.Equals("") ? null : Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Accessory2/{looks.accessory2Type}");
                    part.gameObject.SetActive(!looks.accessory2Type.Equals(""));
                    break;
            }
            part.SetParts(temp);
        }
    }
}
