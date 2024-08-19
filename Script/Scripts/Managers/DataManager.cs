using System.Collections.Generic;
using UnityEngine;

public interface ILoader<Key, Value>
{
    Dictionary<Key, Value> MakeDict();
}

//Json data
public class DataManager : Singleton<DataManager>
{
    public Dictionary<string, ItemData> ItemDatas { get; private set; } = new Dictionary<string, ItemData>();
    public Dictionary<string, FarmData> FarmDatas { get; private set; } = new Dictionary<string, FarmData>();
    public Dictionary<string, FishData> FishDatas { get; private set; } = new Dictionary<string, FishData>();
    public Dictionary<string, List<NPCRoutine>> NPCRoutineDict { get; private set; } = new Dictionary<string, List<NPCRoutine>>();
    public Dictionary<string, NPCDialogueData> NPCDialogueContainer = new Dictionary<string, NPCDialogueData>();
    public Dictionary<int, List<WeatherCreate>> WeatherCreateDict { get; private set; } = new Dictionary<int, List<WeatherCreate>>();
    public Dictionary<int, BuildingInfo> BuildingInfoDict { get; private set; } = new Dictionary<int, BuildingInfo>();

    public override void Init()
    {
        ItemDatas = LoadJson<ItemDataLoader, string, ItemData>("ItemData").MakeDict();
        FarmDatas = LoadJson<FarmDataLoader, string, FarmData>("FarmData").MakeDict();
        FishDatas = LoadJson<FishDataLoader, string, FishData>("FishData").MakeDict();
        NPCRoutineDict = LoadJson<NPCDataLoader, string, List<NPCRoutine>>(typeof(NPCRoutine).Name).MakeDict();
        NPCDialogueContainer = LoadJson<NPCDialogueDataLoader, string, NPCDialogueData>("NPCDialogueContainer").MakeDict();
        WeatherCreateDict = LoadJson<WeatherCreateDataLoader, int, List<WeatherCreate>>("WeatherCreateData").MakeDict();
        BuildingInfoDict = LoadJson<InitialBuildingInfoDataLoader, int, BuildingInfo>("WorldBuildingInfo").MakeDict();
    }

    Loader LoadJson<Loader, Key, Value>(string path) where Loader : ILoader<Key, Value>
    {
        TextAsset textAsset = Managers.Resource.Load<TextAsset>($"Data/Json/{path}");
        return JsonUtility.FromJson<Loader>(textAsset.text);
    }
}