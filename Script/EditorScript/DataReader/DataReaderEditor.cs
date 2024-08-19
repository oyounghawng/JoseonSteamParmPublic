using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Denba.Path;
using System;
using UnityEngine.Networking;
using System.Threading.Tasks;


#if UNITY_EDITOR
using UnityEditor;

// ScriptableObject로 관리하기 위한 DataReader
public class DataReaderEditor : EditorWindow
{
    private static DataReaderEditor instance;

    #region DATA FIELD 
    public NPCDataSO currentAsset;

    #endregion
    Dictionary<string, bool> isOpened = new Dictionary<string, bool>();

    private Vector2 windowScrollPosition = Vector2.zero;
    private Vector2 sceneViewScrollPosition = Vector2.zero;

    private int selectIndex = 0;

    private List<string> datas = new List<string>();

    [MenuItem("NPC/UpdateNPCRoutine")]
    public static async Task UpdateNPCRoutineAsync()
    {
        foreach (var sheet in PathDataSO.SharedPath.GSheets)
        {
            var url = $"{PathDataSO.SharedPath.SpreadSheetURL}export?format=tsv&gid={sheet.sheetName}";
            var req = UnityWebRequest.Get(url);
            var op = req.SendWebRequest();
            await op;
            var res = req.downloadHandler.text;
            var routineDic = res.TsvToDic();

            Dictionary<string, List<NPCRoutine>> dic = new Dictionary<string, List<NPCRoutine>>();
            string rcode = "";
            List<NPCRoutine> routines = new List<NPCRoutine>();

            NPCRoutine routineData = null;
            foreach (var routine in routineDic)
            {
                var keys = routine.Keys.ToArray();
                routineData = new NPCRoutine();
                var fields = routineData.GetType().GetFields();

                if (rcode != "" && rcode != routine[keys[0]])
                {
                    dic.Add(rcode, routines);
                    routines = new List<NPCRoutine>();
                }
                rcode = routine[keys[0]];

                for (int i = 0; i < fields.Length; i++)
                {
                    for (int j = 0; j < keys.Length; j++)
                    {
                        if (fields[i].Name.Equals(keys[j]))
                        {
                            if (fields[i].FieldType == typeof(int))
                            {
                                if (fields[i].Name.Equals("startTime") || fields[i].Name.Equals("endTime"))
                                {
                                    int hour = int.Parse(routine[keys[j]].Substring(0, 2));
                                    int minute = int.Parse(routine[keys[j]].Substring(2, 2));

                                    int allTime = ((hour * 60) + minute);
                                    fields[i].SetValue(routineData, allTime);
                                }
                                else
                                {
                                    fields[i].SetValue(routineData, int.Parse(routine[keys[j]]));
                                }
                                break;
                            }
                            else if (fields[i].FieldType == typeof(float))
                            {
                                fields[i].SetValue(routineData, float.Parse(routine[keys[j]]));
                                break;
                            }
                            else if (fields[i].FieldType == typeof(string))
                            {
                                fields[i].SetValue(routineData, routine[keys[j]]);
                                break;
                            }
                            else if (fields[i].FieldType == typeof(Vector3))
                            {
                                fields[i].SetValue(routineData, routine[keys[j]].ToVector3());
                                break;
                            }
                            else if (fields[i].FieldType.BaseType == typeof(Enum))
                            {
                                fields[i].SetValue(routineData, Enum.Parse(fields[i].FieldType, routine[keys[j]]));
                                break;
                            }
                        }
                    }
                }
                routines.Add(routineData);
            }
            dic.Add(rcode, routines);

            string json = dic.ToJson(true);
            string path = $"{Application.dataPath}/Resources/Data/Json/NPCRoutine.json";
            System.IO.File.WriteAllText(path, json);
            AssetDatabase.Refresh();
        }
    }

    [MenuItem("NPC/Load NPC Dialogue")]
    public static async Task LoadNPCDialogue()
    {
        /* NPC Choice */
        var url1 = $"{PathDataSO.SharedPath.SpreadSheetURL}export?format=tsv&gid=1970178074";
        var req1 = UnityWebRequest.Get(url1);
        var op1 = req1.SendWebRequest();
        await op1;
        var res1 = req1.downloadHandler.text;
        var dialogueChoice = res1.TsvToDic();

        Dictionary<string, List<NPCChoice>> choiceDic = new Dictionary<string, List<NPCChoice>>();

        string rcode2 = "";
        List<NPCChoice> choiceList = new List<NPCChoice>();

        /* Choice 정렬 */
        foreach (var dc in dialogueChoice)
        {
            NPCChoice choice = new NPCChoice();

            var keys = dc.Keys.ToList();
            var fields2 = choice.GetType().GetFields();

            if (rcode2 != "" && dc[keys[1]] != rcode2)
            {
                choiceDic.Add(rcode2, choiceList);
                choiceList = new List<NPCChoice>();
            }
            rcode2 = dc[keys[1]];
            foreach (var key in keys)
            {
                for (int i = 0; i < fields2.Length; i++)
                {
                    if (key.Equals(fields2[i].Name))
                    {
                        if (fields2[i].FieldType == typeof(int))
                        {
                            fields2[i].SetValue(choice, int.Parse(dc[key]));

                        }
                        else if (fields2[i].FieldType == typeof(string))
                        {
                            fields2[i].SetValue(choice, dc[key]);
                        }
                        else
                        {
                            if (dc[key].Contains('|'))
                            {
                                if (fields2[i].Name.Equals("nextDialogue"))
                                {
                                    List<string> context = dc[key].Split('|').ToList();

                                    List<int> intContext = new List<int>();

                                    foreach (var ct in context)
                                    {
                                        if (ct.Equals("") || ct == null)
                                            continue;
                                        intContext.Add(int.Parse(ct));
                                    }
                                    fields2[i].SetValue(choice, intContext);
                                }
                                else
                                {
                                    List<string> context = dc[key].Split('|').ToList();
                                    fields2[i].SetValue(choice, context);
                                }

                            }
                        }
                    }
                }
            }
            choiceList.Add(choice);
        }
        choiceDic.Add(rcode2, choiceList);

        /* NPC Dialgoue */
        var url2 = $"{PathDataSO.SharedPath.SpreadSheetURL}export?format=tsv&gid=1915684158";
        var req2 = UnityWebRequest.Get(url2);
        var op2 = req2.SendWebRequest();
        await op2;
        var res2 = req2.downloadHandler.text;
        var dialogueDic = res2.TsvToDic();

        List<NPCDialogueContainer> containerList = new List<NPCDialogueContainer>();
        List<NPCDialogue> dialogueList = new List<NPCDialogue>();

        NPCDialogueContainer container = Activator.CreateInstance<NPCDialogueContainer>();

        string rcode = "";
        foreach (var dl in dialogueDic)
        {
            NPCDialogue dialogue = new NPCDialogue();

            var keys2 = dl.Keys.ToList();
            if (rcode != "" && dl[keys2[1]] != rcode)
            {
                container.rcode = rcode;
                container.dialogueGroup = dialogueList;
                container.fileName = $"{rcode}_Dialgoue";
                containerList.Add(container);

                dialogueList = new List<NPCDialogue>();
                container = Activator.CreateInstance<NPCDialogueContainer>();
            }
            rcode = dl[keys2[1]];

            var fields = dialogue.GetType().GetFields();

            foreach (var key in keys2)
            {
                for (int i = 0; i < fields.Length; i++)
                {
                    if (key.Equals(fields[i].Name))
                    {
                        if (fields[i].FieldType == typeof(int))
                        {
                            fields[i].SetValue(dialogue, int.Parse(dl[key]));

                        }
                        else if (fields[i].FieldType == typeof(string))
                        {
                            fields[i].SetValue(dialogue, dl[key]);
                        }
                        else
                        {
                            if (dl[key].Contains('|'))
                            {
                                if (fields[i].Name == "choices")
                                {
                                    List<NPCChoice> choice2 = new List<NPCChoice>();

                                    List<string> choices = dl[key].Split('|').ToList();
                                    for (int k = 0; k < choices.Count - 1; k++)
                                    {
                                        for (int j = 0; j < choiceDic[rcode].Count; j++)
                                        {
                                            if (choiceDic[rcode][j].id == int.Parse(choices[k]))
                                            {
                                                choice2.Add(choiceDic[rcode][j]);
                                                break;
                                            }
                                        }
                                    }

                                    fields[i].SetValue(dialogue, choice2);

                                }
                                else
                                {
                                    List<string> context = dl[key].Split('|').ToList();
                                    if (context[context.Count - 1].Equals("") || context[context.Count - 1] == string.Empty)
                                    {
                                        context.RemoveAt(context.Count - 1);
                                    }

                                    fields[i].SetValue(dialogue, context);
                                }

                            }

                        }
                        break;
                    }
                }
            }
            dialogueList.Add(dialogue);
        }

        container.rcode = rcode;
        container.dialogueGroup = dialogueList;
        container.fileName = $"{rcode}_Dialgoue";
        containerList.Add(container);

        Dictionary<string, NPCDialogueContainer> conToJson = new Dictionary<string, NPCDialogueContainer>();
        foreach (var con in containerList)
        {
            conToJson.Add(con.rcode, con);
        }
        string json = conToJson.ToJson(true);
        string path = $"{Application.dataPath}/Resources/Data/Json/NPCDialogueContainer.json";
        System.IO.File.WriteAllText(path, json);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    //[MenuItem("Denba/LoadWeather")]
    //static async Task LoadWeather()
    //{
    //    string url = $"{PathDataSO.SharedPath.SpreadSheetURL}export?format=tsv&gid=362602777";
    //    var req = UnityWebRequest.Get(url);
    //    var op = req.SendWebRequest();
    //    await op;
    //    var res = req.downloadHandler.text;

    //    var data = res.TsvToDic();

    //    string season = "";

    //    Dictionary<Define.Season, List<WeatherCreateData>> weatherDict = new Dictionary<Define.Season, List<WeatherCreateData>>();

    //    List<WeatherCreateData> list = new List<WeatherCreateData>();
    //    foreach(var dt in data)
    //    {
    //        WeatherCreateData weather = new WeatherCreateData();
    //        var keys = dt.Keys.ToArray();
    //        var fields = weather.GetType().GetFields();

    //        if(season != string.Empty && season != dt[keys[0]])
    //        {
    //            weatherDict.Add(list[0].season, list);
    //            list = new List<WeatherCreateData>();
    //        }
    //        season = dt[keys[0]];

    //        foreach (var field in fields)
    //        {
    //            for(int i = 0; i < keys.Length; i++)
    //            {
    //                if (field.Name.Equals(keys[i]))
    //                {
    //                    if(field.FieldType == typeof(int))
    //                    {
    //                        field.SetValue(weather, int.Parse(dt[keys[i]]));
    //                    }
    //                    else if(field.FieldType == typeof(string))
    //                    {
    //                        field.SetValue(weather, dt[keys[i]]);
    //                    }

    //                    else if(field.FieldType.BaseType == typeof(Enum))
    //                    {
    //                        field.SetValue(weather, Enum.Parse(field.FieldType, dt[keys[i]]));
    //                    }

    //                    break;
    //                }
    //            }
    //        }
    //        list.Add(weather);
    //    }
    //    weatherDict.Add(list[0].season, list);

    //    string json = weatherDict.ToJson(true);
    //    string path = $"{Application.dataPath}/Resources/Data/Json/WeatherCreateData.json";
    //    System.IO.File.WriteAllText(path, json);
    //    AssetDatabase.Refresh();

    //}
}
#endif
