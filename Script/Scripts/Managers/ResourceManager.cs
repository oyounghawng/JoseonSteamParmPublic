using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;


public class ResourceManager : MonoSingleton<ResourceManager>
{
    public bool isAutoLoading = false;
    [NonSerialized] public bool isInit = false;

    public async override void Init()
    {
        await LoadAddressable();
    }

    #region Use Resources
#if !USE_COROUINT && !USE_ASYNC
    public Dictionary<string, object> assetPools = new Dictionary<string, object>();
    public T Load<T>(string key) where T : Object
    {
        if (assetPools.ContainsKey(key)) return (T)assetPools[key];
        var asset = Resources.Load<T>(key);
        if (asset != null) assetPools.Add(key, asset);
        return asset;
    }

    public T[] LoadAll<T>(string key) where T : Object
    {
        var assets = Resources.LoadAll<T>(key);

        foreach (var asset in assets)
        {
            if (!assetPools.ContainsKey(asset.name))
                assetPools.Add(asset.name, asset);
        }

        return assets;
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if (original == null)
        {
            Debug.Log($"Failed to load prefab : {path}");
            return null;
        }

        try
        {
            // object pooling
            if (original.GetComponent<Poolable>() != null)
                return Managers.Pool.Pop(original, parent).gameObject;

            GameObject go = Instantiate(original, parent);
            return go;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to instantiate object: {e.Message}");
            return null;
        }
    }

    public GameObject Instantiate(GameObject prefab, Transform parent = null)
    {
        GameObject go = GameObject.Instantiate(prefab, parent);
        go.name = prefab.name;
        return go;
    }

    public void Destroy(GameObject go)
    {
        if (go == null)
            return;

        // object pooling
        Poolable poolable = go.GetComponent<Poolable>();
        if (poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }

#endif
    #endregion

    #region Addressable
    private Dictionary<eAddressableType, Dictionary<string, AddressableMap>> addressableMap = new Dictionary<eAddressableType, Dictionary<string, AddressableMap>>();

    private async void InitAddressableMap()
    {
        await Addressables.LoadAssetsAsync<TextAsset>("AddressableMap", (text) =>
        {
            var map = JsonUtility.FromJson<AddressableMapData>(text.text);
            var key = eAddressableType.prefab;
            Dictionary<string, AddressableMap> mapDic = new Dictionary<string, AddressableMap>();
            foreach (var data in map.list)
            {
                key = data.addressableType;
                if (!mapDic.ContainsKey(data.key))
                    mapDic.Add(data.key, data);
            }
            if (!addressableMap.ContainsKey(key)) addressableMap.Add(key, mapDic);

        }).Task;
        isInit = true;
    }

    public async Task LoadAddressable()
    {
        var init = await Addressables.InitializeAsync().Task;
        var handle = Addressables.DownloadDependenciesAsync("InitDownload");
        await handle.Task;
        switch (handle.Status)
        {
            case AsyncOperationStatus.None:
                break;
            case AsyncOperationStatus.Succeeded:
                Debug.Log("Success");
                break;
            case AsyncOperationStatus.Failed:
                Debug.LogError("failed!");
                break;
            default:
                break;
        }
        Addressables.Release(handle);
        InitAddressableMap();
    }

    public IEnumerator SetProgress(AsyncOperationHandle handle)
    {
        while (!handle.IsDone)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public List<string> GetPaths(string key, eAddressableType addressableType, eAssetType assetType)
    {
        var keys = new List<string>(addressableMap[addressableType].Keys);
        keys.RemoveAll(obj => !obj.Contains(key));
        List<string> retList = new List<string>();
        keys.ForEach(obj =>
        {
            if (addressableMap[addressableType][obj].assetType == assetType)
                retList.Add(addressableMap[addressableType][obj].path);
        });
        return retList;
    }
    public string GetPath(string key, eAddressableType addressableType)
    {
        var map = addressableMap[addressableType][key.ToLower()];
        return map.path;
    }

    public async Task<List<T>> LoadAssets<T>(string key, eAddressableType addressableType, eAssetType assetType)
    {
        try
        {
            var paths = GetPaths(key, addressableType, assetType);
            List<T> retList = new List<T>();
            foreach (var path in paths)
            {
                retList.Add(await LoadAssetAsync<T>(path));
            }
            return retList;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return default;
    }

    public async Task<T> LoadAsset<T>(string key, eAddressableType addressableType)
    {
        try
        {
            var path = GetPath(key, addressableType);
            return await LoadAssetAsync<T>(path);
        }
        catch (Exception e)
        {
            Debug.LogWarning(e.Message);
        }
        return default;
    }

    private async Task<T> LoadAssetAsync<T>(string path)
    {
        try
        {
            if (path.Contains(".prefab") && typeof(T) != typeof(GameObject) || path.Contains("UI/"))
            {
                var obj = await Addressables.LoadAssetAsync<GameObject>(path).Task;
                return obj.GetComponent<T>();
            }
            else
                return await Addressables.LoadAssetAsync<T>(path).Task;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
        return default;
    }
    #endregion


    public void SetNormalNPC()
    {
        ManagerLocator.GetService<NPCCreator>().Init();
        ManagerLocator.GetService<BuildingCreator>().Init();


        
        List<NPCData> npcData = Managers.Game.SaveData.villageData.npcData;

        // NPC가 존재하지 않는다면
        if (Managers.Game.SaveData.villageData.npcData.Count <= 0)
        {
            // TODO : 먼저 Normal NPC 정보를 저장한다.
            for (int i = 0; i < 2; i++)
            {
                var npcObj = Instantiate("NPC/NPC00006");
                NormalNPC normalNPC = npcObj.GetComponent<NormalNPC>();

                // NPC 생성
                normalNPC.NPCData = ManagerLocator.GetService<NPCCreator>().CreateNormalNPCData();
                // 건축물 생성
                var home = ManagerLocator.GetService<BuildingCreator>().CreateNPCHome(normalNPC.NPCData);

                Instantiate(home.gameObject).transform.position = new Vector3(home.buildingInfo.xPos, home.buildingInfo.yPos, 0);
                home.SetOwner(normalNPC);

                normalNPC.NPCData.homePos = new Vector3(home.buildingInfo.xPos, home.buildingInfo.yPos - 5, 0);
                npcData.Add(normalNPC.NPCData);

                npcObj.transform.position = normalNPC.NPCData.homePos;


                Managers.Game.SaveData.villageData.AddPopulation(1);
            }
        }
        else
        {
            for (int i = 0; i < npcData.Count; i++)
            {
                var npcObj = Instantiate("NPC/NPC00006");
                NormalNPC normalNPC = npcObj.GetComponent<NormalNPC>();

                if (npcData[i].type == Define.NPCType.Normal)
                {
                    normalNPC.NPCData = npcData[i];
                    var home = ManagerLocator.GetService<BuildingCreator>().CreateNPCHome(normalNPC.NPCData);
                    Instantiate(home.gameObject).transform.position = new Vector3(home.buildingInfo.xPos, home.buildingInfo.yPos, 0);
                    npcObj.transform.position = normalNPC.NPCData.homePos;
                }
            }
        }
    }

    public void CreateMainNPC()
    {
        ManagerLocator.GetService<NPCCreator>().Init();
        ManagerLocator.GetService<BuildingCreator>().Init();
        MainNPC[] main = Managers.Resource.LoadAll<MainNPC>("NPC");

        var buildingInfo = Managers.Game.SaveData.villageData.buildingInfo;
        foreach (MainNPC npc in main)
        {
            if (!Managers.Game.SaveData.villageData.npcData.Exists(dt => (dt.rcode == npc.NPCData.rcode) && dt.type == Define.NPCType.Normal))
            {
                var home = ManagerLocator.GetService<BuildingCreator>().CreateNPCHome(npc.NPCData);

                npc.NPCData.homePos = new Vector3(home.buildingInfo.xPos, home.buildingInfo.yPos - 5);
                home.buildingInfo.ownerName = npc.NPCData.rcode;

                int pos = buildingInfo.IndexOf(home.buildingInfo);

                buildingInfo.Remove(home.buildingInfo);
                buildingInfo.Insert(pos, home.buildingInfo);
                Instantiate(home.gameObject).transform.position = new Vector3(home.buildingInfo.xPos, home.buildingInfo.yPos, 0);

                Managers.Game.SaveData.villageData.npcData.Add(npc.NPCData);
                Instantiate(npc.gameObject, npc.NPCData.homePos, Quaternion.identity);
                break;
            }
        }
    }

}