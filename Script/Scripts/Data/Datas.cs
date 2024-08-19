using System;
using System.Collections.Generic;

[Serializable]
public enum eAddressableType
{
    prefab,
    tile,
    ui,
    images,
}

public enum eAssetType
{
    sprite = 0,
    jsondata,
    prefab,
    tile
}

[Serializable]
public class AddressableMapData
{
    public List<AddressableMap> list = new List<AddressableMap>();

    public void AddRange(List<AddressableMap> list)
    {
        this.list.AddRange(list);
    }

    public void Add(AddressableMap data)
    {
        list.Add(data);
    }
}

[Serializable]
public class AddressableMap
{
    public eAddressableType addressableType;
    public eAssetType assetType;
    public string key;
    public string path;
}