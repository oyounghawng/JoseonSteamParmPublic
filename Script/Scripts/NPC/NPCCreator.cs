using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.U2D.Animation;
using Random = UnityEngine.Random;

public class NPCCreator
{
    private bool isInit = false;
    private List<string> hat;
    private List<string> hair;
    private List<string> eye;
    private List<string> blush;
    private List<string> lipstick;
    private List<string> body;
    private List<string> top;
    private List<string> under;
    private List<string> shoes;
    private List<string> accessory1;
    private List<string> accessory2;

    public void Init()
    {
        if(!isInit)
        {
            /* 에셋의 이름을 저장 */
            hat = Managers.Resource.LoadAll<SpriteLibraryAsset>("CharacterParts/Hat").Select(asset => asset.name).ToList();
            hair = Managers.Resource.LoadAll<SpriteLibraryAsset>("CharacterParts/Hair").Select(asset => asset.name).ToList();
            eye = Managers.Resource.LoadAll<SpriteLibraryAsset>("CharacterParts/Eye").Select(asset => asset.name).ToList();
            blush = Managers.Resource.LoadAll<SpriteLibraryAsset>("CharacterParts/Blush").Select(asset => asset.name).ToList();
            lipstick = Managers.Resource.LoadAll<SpriteLibraryAsset>("CharacterParts/Lipstick").Select(asset => asset.name).ToList();
            body = Managers.Resource.LoadAll<SpriteLibraryAsset>("CharacterParts/NPCBody").Select(asset => asset.name).ToList();
            top = Managers.Resource.LoadAll<SpriteLibraryAsset>("CharacterParts/Top").Select(asset => asset.name).ToList();
            under = Managers.Resource.LoadAll<SpriteLibraryAsset>("CharacterParts/Under").Select(asset => asset.name).ToList();
            shoes = Managers.Resource.LoadAll<SpriteLibraryAsset>("CharacterParts/Shoes").Select(asset => asset.name).ToList();
            accessory1 = Managers.Resource.LoadAll<SpriteLibraryAsset>("CharacterParts/Accessory1").Select(asset => asset.name).ToList();
            accessory2 = Managers.Resource.LoadAll<SpriteLibraryAsset>("CharacterParts/Accessory2").Select(asset => asset.name).ToList();

            isInit = true;
        }
    }
    public NPCData CreateNormalNPCData()
    {
        NPCData temp = new NPCData();
        temp.rcode = temp.GetHashCode().ToString();
        temp.type = Define.NPCType.Normal;
        int rl = Random.Range(0, 2);
        Define.NPCRole role = (Define.NPCRole)rl;
        temp.role = role;
        int gen = Random.Range(0, (int)Define.Gender.MaxCount);
        Define.Gender gender = (Define.Gender)gen;
        temp.gender = gender;

        switch (gender)
        {
            case Define.Gender.Male:
                temp.displayName = role == Define.NPCRole.Farmer ? "남자 농부" : "남자 어부";
                break;

            case Define.Gender.Female:
                temp.displayName = role == Define.NPCRole.Farmer ? "여자 농부" : "여자 어부";
                break;
        }

        temp.description = $"천민촌에서 활동하는 {temp.displayName}다.";

        temp.looks = CreateNormalNPCLooks(gender, role);

        temp.rank = CreateNPCRank();

        switch (temp.rank)
        {
            case Define.NPCRank.Slave:
                temp.farmingSkillLevel = role == Define.NPCRole.Farmer ? Random.Range(0, 5) : 0;
                temp.fishingSkillLevel = role == Define.NPCRole.Farmer ? 0 : Random.Range(0, 5);
                break;
            case Define.NPCRank.Ordinary:
                temp.farmingSkillLevel = role == Define.NPCRole.Farmer ? Random.Range(5, 10) : 0;
                temp.fishingSkillLevel = role == Define.NPCRole.Farmer ? 0 : Random.Range(5, 10);
                break;
            case Define.NPCRank.Middle:
                temp.farmingSkillLevel = role == Define.NPCRole.Farmer ? Random.Range(10, 15) : 0;
                temp.fishingSkillLevel = role == Define.NPCRole.Farmer ? 0 : Random.Range(10, 15);
                break;
        }
        return temp;
    }

    private CharacterLooks CreateNormalNPCLooks(Define.Gender gender, Define.NPCRole role)
    {
        CharacterLooks looks = new CharacterLooks();

        looks.hatType = role == Define.NPCRole.Farmer ?
            hat.Where(dt => dt.Contains("cowboy")).ToList().RandomRangeName()
            : hat.Where(dt => dt.Contains("lucky")).ToList().RandomRangeName();

        looks.hairType = gender == Define.Gender.Male ?
            hair.Where(dt => dt.Contains("buzzcut")).ToList().RandomRangeName()
            : hair.Where(dt => dt.Contains("braids")).ToList().RandomRangeName();

        looks.eyeType = eye.RandomRangeName();
        looks.blushType = string.Empty;
        looks.lipType = lipstick.RandomRangeName();
        looks.bodyType = body.RandomRangeName();

        looks.topType = gender == Define.Gender.Male ?
            top.Where(dt => dt.Contains("spaghetti")).ToList().RandomRangeName()
            : top.Where(dt => dt.Contains("basic")).ToList().RandomRangeName();

        looks.underType = gender == Define.Gender.Male ? under.Where(dt => dt.Contains("pants")).ToList().RandomRangeName()
            : under.Where(dt => dt.Contains("skirt")).ToList().RandomRangeName();

        looks.shoesType = shoes.RandomRangeName();
        looks.accessory1Type = string.Empty;
        looks.accessory2Type = string.Empty;

        return looks;
    }

    private Define.NPCRank CreateNPCRank()
    {
        Random.InitState((int)DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());
        int count = Random.Range(0, 100);

        if (count < 50)
            return Define.NPCRank.Slave;
        else if (count >= 50 && count < 95)
            return Define.NPCRank.Ordinary;
        else if (count >= 95 && count < 100)
            return Define.NPCRank.Middle;

        return Define.NPCRank.Slave;
    }
}