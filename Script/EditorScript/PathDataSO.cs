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
                // ��ü�� ���� ���
                if(instance == null)
                {
                    // ���ҽ� �������� ������
                    instance = Resources.Load("PathData") as PathDataSO;
                    // Resources ������ �������� ���� ���
                    if(instance == null)
                    {
                        InitFolderPath();
                        // �ش� ������ ���� ����
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
        [Header("��ȯȰ Ŭ���� ��")]
        public string className;

        [Header("��Ʈ�� ID")]
        public string sheetName;

        public string key;

        public List<Dictionary<string, string>> datas;

    }
}