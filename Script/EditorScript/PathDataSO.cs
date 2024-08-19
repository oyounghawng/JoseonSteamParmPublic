using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Denba.Path
{
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class PathDataSO : ScriptableObject
    {
        private static PathDataSO instance;
        public static string ScriptFolderPath { get; private set; }
        public static string ScriptFolderInProjectPath { get; private set; }
        
        public Object scriptableObjectDataFolder;

        public static string ScriptableObjectDataFolder { get => AssetDatabase.GetAssetPath(SharedPath.scriptableObjectDataFolder); }

        public Object npcSpriteLibraryFolder;

        public static string NPCSpriteLibraryFolder { get => AssetDatabase.GetAssetPath(SharedPath.npcSpriteLibraryFolder); }



        public static PathDataSO SharedPath
        {
            get
            {
                // 객체가 없을 경우
                if(instance == null)
                {
                    // 리소스 폴더에서 가져옴
                    instance = Resources.Load("PathData") as PathDataSO;
                    // Resources 폴도에 존재하지 않을 경우
                    if(instance == null)
                    {
                        InitFolderPath();
                        // 해당 에셋을 직접 생성
                        PathDataSO instance = CreateInstance<PathDataSO>();
                        string directory = System.IO.Path.Combine(ScriptFolderInProjectPath, "Resources");
                        if(!System.IO.Directory.Exists(directory))
                            AssetDatabase.CreateFolder(ScriptFolderInProjectPath, "Resources");

                        string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{ScriptFolderInProjectPath}/Resources/PathData.asset");

                        AssetDatabase.CreateAsset(instance, assetPath);
                    }
                }

                return instance;
            }
        }

        [MenuItem("Denba/Path Setting")]
        private static void Select()
        {
            Selection.activeObject = SharedPath;
        }

        private static void InitFolderPath([System.Runtime.CompilerServices.CallerFilePath] string path = "")
        {
            ScriptFolderPath = System.IO.Path.GetDirectoryName(path);
            int index = ScriptFolderPath.IndexOf(@"Assets\");
            if (index > -1)
            {
                ScriptFolderInProjectPath = ScriptFolderPath.Substring(index, ScriptFolderPath.Length - index);
            }
        }

        [Header("Google Sheet Data")]
        public string GoogleSheetUrl;

        public string SpreadSheetURL;

        public List<SheetInfo> sheets;

        public List<GSheet> GSheets;
    }

    [System.Serializable]
    public class SheetInfo
    {
        public string className;
        
        public string sheetName;

        public int startRow;
        public int endRow;

        public string key;
    }

    [System.Serializable]
    public class GSheet
    {
        [Header("변환활 클래스 명")]
        public string className;

        [Header("시트의 ID")]
        public string sheetName;

        public string key;

        public List<Dictionary<string, string>> datas;

    }
}