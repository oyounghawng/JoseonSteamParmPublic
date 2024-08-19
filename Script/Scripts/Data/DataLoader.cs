using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemData
{
    public string rCode;
    public string displayName;
    public string description;
    public int type;
    public bool canStack;
    public int maxStackAmount;
    public int sellPrice;
    public int buyPrice;
    public string relationData;
    public string equipPrefab;
}

[Serializable]
public class ItemDataLoader : ILoader<string, ItemData>
{
    public List<ItemData> items = new List<ItemData>();

    public Dictionary<string, ItemData> MakeDict()
    {
        Dictionary<string, ItemData> dic = new Dictionary<string, ItemData>();

        foreach (ItemData item in items)
            dic.Add(item.rCode, item);

        return dic;
    }
}

[Serializable]
public class FishData
{
    public string rCode;
    public string EName;
    public string KName;
    public List<int> Season;
    public string Spot;
    public int Difficulty;
    public int Tool;
    public string RewardItem;
}

[Serializable]
public class FishDataLoader : ILoader<string, FishData>
{
    public List<FishData> fishes = new List<FishData>();

    public Dictionary<string, FishData> MakeDict()
    {
        Dictionary<string, FishData> dic = new Dictionary<string, FishData>();

        foreach (FishData fish in fishes)
            dic.Add(fish.rCode, fish);

        return dic;
    }
}

[Serializable]
public class FarmData
{
    public string rCode;
    public string EName;
    public string KName;
    public string Season;
    public int GrowTime;
    public string RewardItem;
}

[Serializable]
public class FarmDataLoader : ILoader<string, FarmData>
{
    public List<FarmData> farms = new List<FarmData>();

    public Dictionary<string, FarmData> MakeDict()
    {
        Dictionary<string, FarmData> dic = new Dictionary<string, FarmData>();

        foreach (FarmData farm in farms)
            dic.Add(farm.rCode, farm);

        return dic;
    }
}

#region NPC

public class WorldNPCData
{
    public List<NPCData> npcData = new List<NPCData> { new NPCData() };
}
[Serializable]
public class NPCRoutine
{
    public string rcode;

    public int dayofweek;

    // 언제부터 언제까지
    public int startTime;

    public int endTime;

    // 어디서
    public Vector3 destination;

    // 어느 방향을 보면서
    public Vector3 direction;

    // 무엇을
    public Define.NPCBehavior behavior;
}

[Serializable]
public class NPCRoutineData
{
    public string Key;
    public List<NPCRoutine> Value;
}

[Serializable]
public class NPCDataLoader : ILoader<string, List<NPCRoutine>>
{
    public List<NPCRoutineData> data = new List<NPCRoutineData>();

    public Dictionary<string, List<NPCRoutine>> MakeDict()
    {
        Dictionary<string, List<NPCRoutine>> dic = new Dictionary<string, List<NPCRoutine>>();

        foreach (var dt in data)
        {
            dic.Add(dt.Key, dt.Value);
        }
        return dic;
    }
}

#endregion

#region NPC Dialogue

[Serializable]
public class NPCDialogueData
{
    public string Key;
    public NPCDialogueContainer Value;
}

[Serializable]
public class NPCDialogueContainer
{
    public string rcode;
    public string fileName;
    public List<NPCDialogue> dialogueGroup;
}

[Serializable]
public class NPCDialogue
{
    public int id;
    public string rcode;
    public string displayName;
    public string key;
    public List<string> context;
    public List<NPCChoice> choices;
}

[Serializable]
public class NPCChoice
{
    public int id;
    public List<string> choiceText;
    public List<int> nextDialogue;
}

public class NPCDialogueDataLoader : ILoader<string, NPCDialogueData>
{
    public List<NPCDialogueData> data = new List<NPCDialogueData>();

    public Dictionary<string, NPCDialogueData> MakeDict()
    {
        Dictionary<string, NPCDialogueData> dic = new Dictionary<string, NPCDialogueData>();

        foreach (var dt in data)
        {
            dic.Add(dt.Key, dt);
        }

        return dic;

    }
}
#endregion

#region Sprite Library Assset
[Serializable]
public class PartsLabelSprite
{
    public string category;
    public string[] labels;
    public Sprite[] sprites;
}

[Serializable]
public class PartsLabelSpriteData
{
    public int Key;
    public PartsLabelSprite Value;
}

[Serializable]
public class PartsLabelSpriteLoader : ILoader<int, PartsLabelSprite>
{
    public List<PartsLabelSprite> data = new List<PartsLabelSprite>();
    public Dictionary<int, PartsLabelSprite> MakeDict()
    {
        Dictionary<int, PartsLabelSprite> dic = new Dictionary<int, PartsLabelSprite>();

        for (int i = 0; i < data.Count; i++)
        {
            dic.Add(i, data[i]);
        }
        return dic;
    }
}
#endregion

#region Weather

[Serializable]
public class WeatherCreate
{
    public int season;
    public int day;
    public int weather;
    public int probability;
}

[Serializable]
public class WeatherCreateData
{
    public int Key;
    public List<WeatherCreate> Value;
}

[Serializable]
public class WeatherCreateDataLoader : ILoader<int, List<WeatherCreate>>
{
    public List<WeatherCreateData> data = new List<WeatherCreateData>();

    public Dictionary<int, List<WeatherCreate>> MakeDict()
    {
        Dictionary<int, List<WeatherCreate>> dict = new Dictionary<int, List<WeatherCreate>>();

        foreach (var dt in data)
        {
            dict.Add(dt.Key, dt.Value);
        }
        return dict;
    }
}
#endregion

#region Building

[Serializable]
public class BuildingInfo
{
    public int id;
    public string buildingName;
    public float xPos;
    public float yPos;
    public string ownerName;
}

[Serializable]
public class WorldBuildingInfo
{
    public List<BuildingInfo> buildingInfo;
}

public class InitialBuildingInfoDataLoader : ILoader<int, BuildingInfo>
{
    public List<BuildingInfo> buildingInfo = new List<BuildingInfo>();
    public Dictionary<int, BuildingInfo> MakeDict()
    {
        Dictionary<int, BuildingInfo> dict = new Dictionary<int, BuildingInfo>();
        foreach (var dt in buildingInfo)
        {
            dict.Add(dt.id, dt);
        }
        return dict;
    }
}
#endregion
