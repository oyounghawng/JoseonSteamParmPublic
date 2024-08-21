# Data
- 
    <aside>
    💡 **Json을 활용하여 데이터 관리**
    
    ---
    
    *📝 **DataManager에 모든 데이터를 저장***
    
    - 필요 데이터를 Json으로 저장해 두고 이를 Dictionary 형태로 DataManager에 키값을 rCode를 이용해 저장하고 접근하여 사용
    - 코드
        
        ```css
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
        ```
        
    - 각각의 필요 데이터를 클래스로 선언하여 Json파일의 내용들을 불러와 저장해서 사용
    - 코드
        
        ```css
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
        ```
        
    
    📝 **에디터 툴의 사용**
    
    ---
    
    - CSV 파일의 변동사항을 Json에 바로 저장하는 에디터 툴 사용
    - 코드
        
        ```css
        static void ParseFishData(string filename)
        {
            FishDataLoader loader = new FishDataLoader();
        
            #region ExcelData
            string[] lines = File.ReadAllText($"{Application.dataPath}/Resources/Data/Excel/{filename}Data.csv").Split("\n");
        
            for (int y = 1; y < lines.Length; y++)
            {
                string[] row = lines[y].Replace("\r", "").Split(',');
                if (row.Length == 0)
                    continue;
                if (string.IsNullOrEmpty(row[0]))
                    continue;
                int i = 0;
        
                loader.fishes.Add(new FishData()
                {
                    rCode = (row[i++]),
                    EName = (row[i++]),
                    KName = (row[i++]),
                    Season = ParseSeason(row[i++]),
                    Spot = (row[i++]),
                    Difficulty = int.Parse(row[i++]),
                    Tool = int.Parse(row[i++]),
                    RewardItem = (row[i++]),
                });
            }
        
            #endregion
        
            string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
            File.WriteAllText($"{Application.dataPath}/Resources/Data/Json/{filename}Data.json", jsonStr);
            AssetDatabase.Refresh();
        }
        ```
        
    - Prefab의 이름을 통해 ItemObject클래스에 필요한 데이터를 넣어주는 에디터 툴 사용
    - 코드
        
        ```css
            [MenuItem("Tools/ResetPrefabData")]
            public static void LoadprefabData()
            {
                DataManager.Instance.Init();
        
                string folderPath = "Assets/AddressableDatas/Prefab";
                string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab", new[] { folderPath });
        
                List<GameObject> prefabs = new List<GameObject>();
        
                List<Sprite> fishsprites = LoadAllTexturesFromFolder("Assets/Resources/Fish");
        
                foreach (string guid in prefabGuids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                    if (prefab != null)
                    {
                        prefabs.Add(prefab);
                    }
        
                    if (prefab == null)
                        continue;
        
                    // 프리팹의 모든 컴포넌트를 순회합니다.
                    ItemObject itemObject;
                    if (prefab.TryGetComponent(out itemObject))
                    {
                        itemObject.itemObjectData.itemKey = prefab.name;
                        DataManager.Instance.ItemDatas.TryGetValue(itemObject.itemObjectData.itemKey, out itemObject.itemObjectData.itemData);
                        // 이미지설정
                        if (itemObject.itemObjectData.itemData.type == (int)Define.ItemType.FarmItem)
                        {
                            FarmData farmData;
                            DataManager.Instance.FarmDatas.TryGetValue(itemObject.itemObjectData.itemData.relationData, out farmData);
                            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/Resources/Farm/Crops/{farmData.Season}/{farmData.EName}.png");
                            List<Sprite> farmSprites = LoadAllSpritesFromTexture(texture);
                            Sprite sprite = itemObject.itemObjectData.itemData.displayName.Contains("씨앗") ? farmSprites[0] : farmSprites[1];
                            itemObject.itemObjectData.image = sprite;
                            prefab.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
                            if (itemObject.itemObjectData.itemData.displayName.Contains("씨앗"))
                            {
                                SetTileFolder(farmSprites, $"Assets/Resources/Farm/CropsTile/{farmData.EName}");
                                itemObject.itemObjectData.tiles = LoadAllTiles($"Assets/Resources/Farm/CropsTile/{farmData.EName}");
                            }
                        }
                        else if (itemObject.itemObjectData.itemData.type == (int)Define.ItemType.FishingItem)
                        {
                            FishData fishdata;
                            DataManager.Instance.FishDatas.TryGetValue(itemObject.itemObjectData.itemData.relationData, out fishdata);
                            Sprite sprite = GetSpritesFromPath(fishsprites, fishdata.EName);
                            itemObject.itemObjectData.image = sprite;
                            Sprite silluetSprite = GetSpritesFromPath(fishsprites, $"{fishdata.EName} shadow");
                            itemObject.itemObjectData.silluetImage = silluetSprite;
                            prefab.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
                        }
                    }
        
                    PrefabUtility.SaveAsPrefabAsset(prefab, path);
                }
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        ```
        
    </aside>
    
