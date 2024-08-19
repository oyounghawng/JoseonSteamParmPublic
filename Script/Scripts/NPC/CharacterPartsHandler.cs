using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class CharacterPartsHandler : MonoBehaviour
{
    public CharacterLooks looks;
    public NPC npc;
    [SerializeField]
    private List<CharacterParts> characterParts;

    private void Awake()
    {
        characterParts = GetComponentsInChildren<CharacterParts>().ToList();
    }
    private void Start()
    {
        looks = npc.NPCData.looks;
        if (looks != null)
            Init();
    }
    public void Init()
    {
        AutoClothing();
    }
    public void AutoClothing()
    {
        SpriteLibraryAsset temp = null;
        foreach (var part in characterParts)
        {
            switch (part.parts)
            {
                case Define.CharacterParts.Hat:
                    temp = looks.hairType.Equals("") ? null : Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Hat/{looks.hatType}");
                    part.gameObject.SetActive(!looks.hatType.Equals(""));
                    break;
                case Define.CharacterParts.Hair:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Hair/{looks.hairType}");
                    break;
                case Define.CharacterParts.Eye:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Eye/{looks.eyeType}");
                    break;
                case Define.CharacterParts.Blush:
                    temp = looks.blushType.Equals("") ? null : Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Blush/{looks.blushType}");
                    part.gameObject.SetActive(!looks.blushType.Equals(""));
                    break;
                case Define.CharacterParts.Lipstick:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Lipstick/{looks.lipType}");
                    break;
                case Define.CharacterParts.Body:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/NPCBody/{looks.bodyType}");
                    break;
                case Define.CharacterParts.Top:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Top/{looks.topType}");
                    break;
                case Define.CharacterParts.Under:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Under/{looks.underType}");
                    break;
                case Define.CharacterParts.Shoes:
                    temp = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/Shoes/{looks.shoesType}");
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

    public void SetLooks(CharacterLooks looks)
    {
        this.looks = looks;

        AutoClothing();
    }
}
