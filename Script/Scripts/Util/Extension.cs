using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.Tilemaps;

public static class Extension
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        return Util.GetOrAddComponent<T>(go);
    }

    public static void BindEvent(this GameObject go, Action<PointerEventData> action, Define.UIEvent type = Define.UIEvent.Click)
    {
        UI_Base.BindEvent(go, action, type);
    }

    public static bool IsValid(this GameObject go)
    {
        return go != null && go.activeSelf;
    }

    public static Vector3 ToVector3(this string str)
    {
        if (str[0] == '(' && str.Last() == ')')
        {
            var pos = str.Substring(1, str.Length - 2).Split(',');
            return new Vector3(float.Parse(pos[0]), float.Parse(pos[1]), float.Parse(pos[2]));
        }
        return Vector3.zero;
    }

    public static void Clear<T>(this T[,] array)
    {
        int rows = array.GetUpperBound(0);
        int cols = array.GetUpperBound(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                array[i, j] = default(T);
            }
        }
    }

    public static void ArraySwap<T>(this T[] FromArray, T[] ToArray, int from, int to)
    {
        T tmp = ToArray[to];
        ToArray[to] = FromArray[from];
        FromArray[from] = tmp;
    }
    public static string FileName(this string path)
    {
        return path.Split('/').Last().Split('.')[0].ToLower();
    }

    public static IEnumerator CountNumber(this int number, TMP_Text text, int speedMultiplier = 4)
    {
        int i = 0;
        int increment = Mathf.Max(1, number / 60 * speedMultiplier); // 최소 1로 설정

        while (i < number)
        {
            text.text = $"{i}";
            i += increment;
            if (i > number) i = number;
            yield return CoroutineHelper.WaitForSeconds(1f / 60f);
        }

        text.text = $"{number}";
    }

    public static string RandomRangeName<T>(this List<T> list)
    {
        if (list == null || list.Count == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex].ToString(); // T를 string으로 변환하여 반환
    }



    #region Add GetAwaiter To UnityWebRequest

    public static TaskAwaiter<UnityWebRequest.Result> GetAwaiter(this UnityWebRequestAsyncOperation reqOp)
    {
        TaskCompletionSource<UnityWebRequest.Result> tsc = new TaskCompletionSource<UnityWebRequest.Result>();
        reqOp.completed += asyncOp => tsc.TrySetResult(reqOp.webRequest.result);

        if (reqOp.isDone)
            tsc.TrySetResult(reqOp.webRequest.result);

        return tsc.Task.GetAwaiter();
    }

    #endregion

    #region Dictionary Serialization / DeSerialization
    public static string ToJson<TKey, TValue>(this Dictionary<TKey, TValue> jsonDicData, bool pretty = false)
    {
        List<DataDictionary<TKey, TValue>> dataList = new List<DataDictionary<TKey, TValue>>();
        DataDictionary<TKey, TValue> dictionaryData;
        foreach (TKey key in jsonDicData.Keys)
        {
            dictionaryData = new DataDictionary<TKey, TValue>();
            dictionaryData.Key = key;
            dictionaryData.Value = jsonDicData[key];
            dataList.Add(dictionaryData);
        }
        JsonDataArray<TKey, TValue> arrayJson = new JsonDataArray<TKey, TValue>();
        arrayJson.data = dataList;

        return JsonUtility.ToJson(arrayJson, pretty);
    }


    // List<Dictionary<int, List<Item>>>
    public static Dictionary<TKey, TValue> FromJson<TKey, TValue>(this string jsonData)
    {
        List<DataDictionary<TKey, TValue>> dataList = JsonUtility.FromJson<List<DataDictionary<TKey, TValue>>>(jsonData);

        Dictionary<TKey, TValue> returnDictionary = new Dictionary<TKey, TValue>();

        for (int i = 0; i < dataList.Count; i++)
        {
            DataDictionary<TKey, TValue> dictionaryData = dataList[i];
            returnDictionary[dictionaryData.Key] = dictionaryData.Value;
        }

        return returnDictionary;

    }

    #endregion

    #region GSpread Sheet Function
    public static List<Dictionary<string, string>> TsvToDic(this string data)
    {
        List<Dictionary<string, string>> list = new List<Dictionary<string, string>>();
        var rows = data.Split('\n');
        var keys = rows[0].Trim().Split('\t');
        for (int i = 1; i < rows.Length; i++)
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var columns = rows[i].Trim().Split('\t');
            for (int j = 0; j < columns.Length; j++)
            {
                dic.Add(keys[j], columns[j]);
            }
            list.Add(dic);
        }
        return list;
    }

    public static T DicToClass<T>(this Dictionary<string, string> data)
    {
        var dt = Activator.CreateInstance(typeof(T));
        var fields = typeof(T).GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        var keys = new List<string>(data.Keys);
        foreach (var field in fields)
        {
            try
            {
                var idx = keys.FindIndex(obj => obj == field.Name);
                if (idx >= 0)
                {
                    if (field.FieldType == typeof(int))
                    {
                        field.SetValue(dt, int.Parse(data[keys[idx]]));
                    }
                    else if (field.FieldType == typeof(float))
                    {
                        field.SetValue(dt, float.Parse(data[keys[idx]]));
                    }
                    else if (field.FieldType == typeof(bool))
                    {
                        field.SetValue(dt, bool.Parse(data[keys[idx]]));
                    }
                    else if (field.FieldType == typeof(double))
                    {
                        field.SetValue(dt, double.Parse(data[keys[idx]]));
                    }
                    else if (field.FieldType == typeof(string))
                    {
                        field.SetValue(dt, data[keys[idx]]);
                    }
                    else if (field.FieldType == typeof(List<int>))
                    {
                        var datas = data[keys[idx]].Split('|');
                        List<int> list = new List<int>();
                        foreach (var str in datas)
                        {
                            list.Add(int.Parse(str));
                        }
                        field.SetValue(dt, list);
                    }
                    else if (field.FieldType == typeof(int[]))
                    {
                        var datas = data[keys[idx]].Split('|');
                        List<int> list = new List<int>();
                        foreach (var str in datas)
                        {
                            list.Add(int.Parse(str));
                        }
                        field.SetValue(dt, list.ToArray());
                    }
                    else if (field.FieldType == typeof(List<string>))
                    {
                        field.SetValue(dt, data[keys[idx]].Split('|').ToList());
                    }
                    else if (field.FieldType == typeof(string[]))
                    {
                        field.SetValue(dt, data[keys[idx]].Split('|'));
                    }
                    else if (field.FieldType == typeof(Vector3))
                    {
                        field.SetValue(dt, data[keys[idx]].ToVector3());
                    }
                    else
                    {
                        field.SetValue(dt, Enum.Parse(field.FieldType, data[keys[idx]]));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Log("Convert Failed" + ex);
            }
        }
        return (T)dt;
    }

    #endregion

    #region Custom DoTween

    public static TweenerCore<string, string, StringOptions> DOTMPText(this TMP_Text target, string endValue, float duration, bool richTextEnabled = true, ScrambleMode scrambleMode = ScrambleMode.None, string scrambleChars = null)
    {
        if (endValue == null)
        {
            if (Debugger.logPriority > 0) Debugger.LogWarning("You can't pass a NULL string to DOText: an empty string will be used instead to avoid errors");
            endValue = "";
        }
        TweenerCore<string, string, StringOptions> t = DOTween.To(() => target.text, x => target.text = x, endValue, duration);
        t.SetOptions(richTextEnabled, scrambleMode, scrambleChars)
            .SetTarget(target);
        return t;
    }




    #endregion


    #region Tilemap Extension

    public static Vector3Int WorldToCell(this Vector3 pos, Tilemap tilemap)
    {
        return tilemap.WorldToCell(pos);
    }

    public static Vector3 CellToWorld(this Vector3Int pos, Tilemap tilemap)
    {
        return tilemap.CellToWorld(pos);
    }

    #endregion
}
