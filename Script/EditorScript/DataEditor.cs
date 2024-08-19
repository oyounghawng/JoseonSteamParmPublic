using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Object = UnityEngine.Object;

public class DataEditor : EditorWindow
{
#if UNITY_EDITOR
    [MenuItem("Tools/DeleteGameData")]
    public static void DeleteGameData()
    {
        string path = Application.persistentDataPath + "/SaveData.json";
        if (File.Exists(path))
            File.Delete(path);

        Debug.Log("�÷��� �����Ͱ� �����Ǿ����ϴ�.");
    }

    [MenuItem("Tools/DeletePlayerPrefs")]
    public static void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();

        Debug.Log("�÷��̾� �����ս� �����Ͱ� �����Ǿ����ϴ�.");
    }

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

            // �������� ��� ������Ʈ�� ��ȸ�մϴ�.
            ItemObject itemObject;
            if (prefab.TryGetComponent(out itemObject))
            {
                itemObject.itemObjectData.itemKey = prefab.name;
                DataManager.Instance.ItemDatas.TryGetValue(itemObject.itemObjectData.itemKey, out itemObject.itemObjectData.itemData);
                // �̹�������
                if (itemObject.itemObjectData.itemData.type == (int)Define.ItemType.FarmItem)
                {
                    FarmData farmData;
                    DataManager.Instance.FarmDatas.TryGetValue(itemObject.itemObjectData.itemData.relationData, out farmData);
                    Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>($"Assets/Resources/Farm/Crops/{farmData.Season}/{farmData.EName}.png");
                    List<Sprite> farmSprites = LoadAllSpritesFromTexture(texture);
                    Sprite sprite = itemObject.itemObjectData.itemData.displayName.Contains("����") ? farmSprites[0] : farmSprites[1];
                    itemObject.itemObjectData.image = sprite;
                    prefab.GetComponentInChildren<SpriteRenderer>().sprite = sprite;
                    if (itemObject.itemObjectData.itemData.displayName.Contains("����"))
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
    public static List<TileBase> LoadAllTiles(string folderPath)
    {
        List<TileBase> tiles = new List<TileBase>();

        // ��ο��� ��� Ÿ�� ������ ��θ� �����ɴϴ�.
        string[] assetPaths = AssetDatabase.FindAssets("t:Tile", new[] { folderPath });

        foreach (string assetPath in assetPaths)
        {
            // ������ ��ü ��θ� ����ϴ�.
            string path = AssetDatabase.GUIDToAssetPath(assetPath);

            // Ÿ�� ������ �ε��մϴ�.
            TileBase tile = AssetDatabase.LoadAssetAtPath<TileBase>(path);

            if (tile != null)
            {
                tiles.Add(tile);
            }
        }

        return tiles;
    }
    private static void SetTileFolder(List<Sprite> sprites, string path)
    {
        string[] folders = path.Split('/');
        string parentPath = "Assets";
        string folderName = folders[folders.Length - 1];
        if (!AssetDatabase.IsValidFolder(path))
        {
            for (int i = 1; i < folders.Length - 1; i++)
            {
                parentPath += "/" + folders[i];
            }
            //���ٸ� ����
            AssetDatabase.CreateFolder(parentPath, folderName);
        }
        else
        {
            Debug.Log("Folder already exists at: " + path);
        }

        //�ش��ο� ������ �ִٸ� �ش�Ǵ� Ÿ�ϵ��� ���� or �������ֱ�
        for (int i = 1; i < sprites.Count; i++)
        {
            // Ÿ�� ������ ������ ��ο� ���� �̸� ����
            string tilepath = $"Assets/Resources/Farm/CropsTile/{folderName}/{folderName}_{i - 1}.asset";
            if (File.Exists(path))
                continue;
            Tile newTile = CreateInstance<Tile>();
            // Ÿ���� ��������Ʈ ����
            newTile.sprite = sprites[i];
            // Ÿ�� ������ ���Ϸ� ����
            AssetDatabase.CreateAsset(newTile, tilepath);
        }
    }

    private static Sprite GetSpritesFromPath(List<Sprite> sprites, string name)
    {
        Sprite targetSprite = sprites.Find(sprite => sprite.name.Equals(name));
        return targetSprite;
    }

    private static List<Sprite> LoadAllSpritesFromTexture(Texture2D texture)
    {
        // �ؽ�ó�� ��θ� �����ɴϴ�.
        string texturePath = AssetDatabase.GetAssetPath(texture);

        // �ش� ��ο��� ��� �ڻ��� �ε��մϴ�.
        Object[] spriteAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(texturePath);

        // Object �迭�� Sprite �迭�� ��ȯ�մϴ�.
        List<Sprite> sprites = new List<Sprite>();
        foreach (Object asset in spriteAssets)
        {
            if (asset is Sprite sprite)
            {
                sprites.Add(sprite);
            }
        }

        return sprites;
    }

    private static List<Sprite> LoadAllTexturesFromFolder(string folderPath)
    {
        List<Sprite> sprites = new List<Sprite>();

        // ���� �� ��� �ڻ��� GUID�� ã���ϴ�.
        string[] assetGuids = AssetDatabase.FindAssets("t:Texture2D", new[] { folderPath });

        // �� �ڻ� GUID�� ���� ��θ� ã�� �ؽ�ó �ε�
        foreach (string guid in assetGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(assetPath);
            Object[] spriteAssets = AssetDatabase.LoadAllAssetRepresentationsAtPath(AssetDatabase.GetAssetPath(texture));
            foreach (Object asset in spriteAssets)
            {
                if (asset is Sprite sprite)
                {
                    sprites.Add(sprite);
                }
            }
        }
        return sprites;
    }


    #region csv to json
    [MenuItem("Tools/ParseExcel")]
    public static void ParseExcel()
    {
        ParseItemData("Item");
        ParseFishData("Fish");
        ParseFarmData("Farm");
    }
    static void ParseItemData(string filename)
    {
        ItemDataLoader loader = new ItemDataLoader();

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

            loader.items.Add(new ItemData()
            {
                rCode = (row[i++]),
                displayName = (row[i++]),
                description = (row[i++]),
                type = int.Parse(row[i++]),
                canStack = (row[i++]) == "TRUE" ? true : false,
                maxStackAmount = int.Parse(row[i++]),
                sellPrice = int.Parse(row[i++]),
                buyPrice = int.Parse(row[i++]),
                relationData = (row[i++]),
                equipPrefab = (row[i++]),
            });
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/Resources/Data/Json/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }
    #endregion
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

    static void ParseFarmData(string filename)
    {
        FarmDataLoader loader = new FarmDataLoader();

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

            loader.farms.Add(new FarmData()
            {
                rCode = (row[i++]),
                EName = (row[i++]),
                KName = (row[i++]),
                Season = (row[i++]),
                GrowTime = int.Parse(row[i++]),
                RewardItem = (row[i++]),
            });
        }

        #endregion

        string jsonStr = JsonConvert.SerializeObject(loader, Formatting.Indented);
        File.WriteAllText($"{Application.dataPath}/Resources/Data/Json/{filename}Data.json", jsonStr);
        AssetDatabase.Refresh();
    }

    private static List<int> ParseSeason(string seasonString)
    {   
        string[] seasonStrings = seasonString.Split(';');
        List<int> seasons = new List<int>();
        for (int i = 0; i < seasonStrings.Length; i++)
        {
            int season;
            if (int.TryParse(seasonStrings[i], out season))
            {
                seasons.Add(season);
            }
            else
            {
                throw new FormatException($"Season value '{seasonStrings[i]}' is not a valid integer.");
            }
        }
        return seasons;
    }
#endif
}