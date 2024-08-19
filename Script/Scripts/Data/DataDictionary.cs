using System;

[Serializable]
public class DataDictionary<TKey, TValue>
{
    public TKey Key;
    public TValue Value;
}
