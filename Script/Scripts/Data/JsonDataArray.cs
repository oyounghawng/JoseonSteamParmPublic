using System.Collections.Generic;
using System;

[Serializable]
public class JsonDataArray<TKey, TValue>
{
    public List<DataDictionary<TKey, TValue>> data;
}

