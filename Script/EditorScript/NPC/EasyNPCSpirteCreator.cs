using Denba.Path;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D.Animation;

#if UNITY_EDITOR
public static class EasyNPCSpirteCreator
{
    ///// <summary>
    ///// SpriteLibraryAsset을 동적 생성하는 기능
    ///// </summary>
    //[MenuItem("NPC/Create Sprite Lib")]
    //static void CreateSpriteLib()
    //{
    //    var asset = ScriptableObject.CreateInstance<SpriteLibraryAsset>();
    //    AssetDatabase.CreateAsset(asset, AssetDatabase.GenerateUniqueAssetPath("Assets/03_ScriptableObject/Data/NPCSpriteLib/" + "1.asset"));
    //    AssetDatabase.SaveAssets();
    //    AssetDatabase.Refresh();
    //    EditorUtility.SetDirty(asset);
    //}

    ///// <summary>
    ///// NPCDataSO에 저장된 NPC의 생김새 정보에 대한 SpriteLibraryAsset을 자동으로 생성하는 메서드
    ///// </summary>
    //[MenuItem("NPC/Auto Create NPCSpriteLib")]
    //static void AutoCreateNPCSpriteLib()
    //{
    //    // TODO : Load한 NPCDataSO를 가져옵니다.
    //    string soPath = PathDataSO.ScriptableObjectDataFolder + "/NPCDataSO";
    //    string[] guids = AssetDatabase.FindAssets("", new[] { soPath });

    //    NPCDataSO[] soDatas = new NPCDataSO[guids.Length];

    //    for (int i = 0; i < soDatas.Length; i++)
    //    {
    //        string assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);

    //        soDatas[i] = AssetDatabase.LoadAssetAtPath<NPCDataSO>(assetPath);

    //        if (soDatas[i] != null)
    //        {
    //            Debug.Log("데이터 불러오기 성공");
    //        }
    //        else
    //        {
    //            Debug.Log("데이터 불러오기 실패");
    //        }
    //    }

    //    var parts = Resources.LoadAll<SpriteLibraryAsset>("CharacterParts/");

    //    // Assets/06_ScriptableObject/Data/NPCDataSO
    //    foreach (NPCDataSO data in soDatas)
    //    {
    //        var asset = ScriptableObject.CreateInstance<SpriteLibraryAsset>();

    //        var fields = data.GetType().GetFields();

    //        for (int i = 0; i < fields.Length; i++)
    //        {
    //            for (int k = 0; k < parts.Length; k++)
    //            {
    //                if (fields[i].Name.Equals(parts[k].name.ToLower()))
    //                {
    //                    if (fields[i].GetValue(data).ToString().Equals(""))
    //                    {
    //                        asset.AddCategoryLabel(null, parts[k].name, "1");
    //                        continue;
    //                    }

    //                    string[] categories = parts[k].GetCategoryNames().ToArray();
    //                    string[] labels = null;
    //                    Sprite[] sprites = null;

    //                    foreach (string category in categories)
    //                    {
    //                        if (category.Equals(fields[i].GetValue(data).ToString()))
    //                        {
    //                            labels = parts[k].GetCategoryLabelNames(category).ToArray();

    //                            sprites = new Sprite[labels.Length];

    //                            for (int u = 0; u < sprites.Length; u++)
    //                            {
    //                                sprites[u] = parts[k].GetSprite(category, labels[u]);

    //                                asset.AddCategoryLabel(sprites[u], parts[k].name, labels[u]);
    //                            }

    //                        }
    //                    }
    //                }
    //            }
    //        }

    //        Debug.Log(data.name);

    //        AssetDatabase.CreateAsset(asset, $"{PathDataSO.NPCSpriteLibraryFolder}/{data.name}.asset");

    //        AssetDatabase.SaveAssets();
    //        AssetDatabase.Refresh();
    //        EditorUtility.SetDirty(asset);
    //    }

    //}

    //[MenuItem("NPC/AutoSeperateCategoryToSpriteLib")]
    //static void AutoSeperateCategoryToSpriteLib()
    //{
    //    var objs = Selection.objects;

    //    foreach(var obj in objs)
    //    {
    //        if(obj is SpriteLibraryAsset)
    //        {
    //            SpriteLibraryAsset asset = obj as SpriteLibraryAsset;
    //            List<string> categories = asset.GetCategoryNames().ToList();

    //            foreach(string category in categories)
    //            {
    //                SpriteLibraryAsset temp = ScriptableObject.CreateInstance<SpriteLibraryAsset>();

    //                List<string> labels = asset.GetCategoryLabelNames(category).ToList();
                    
    //                for(int i = 0; i < labels.Count; i++)
    //                {
    //                    temp.AddCategoryLabel(asset.GetSprite(category, labels[i]), asset.name, labels[i]);
    //                }
    //                temp.name = category;

    //                string path = $"Assets/Resources/CharacterParts/Temp/{category}.asset";
    //                AssetDatabase.CreateAsset(temp, AssetDatabase.GenerateUniqueAssetPath(path));
    //                EditorUtility.SetDirty(temp);

    //            }
    //        }
    //        AssetDatabase.SaveAssets();
    //        AssetDatabase.Refresh();

    //    }

    //}
    //#region 현재 사용하지 않는 기능들 (삭제하지 마세요)
    ///*
    //[MenuItem("NPC/Add category to Sprite Lib")]
    //static void AddCategryToSpriteLib()
    //{
    //    var objs = Selection.objects;

    //    foreach (var obj in objs)
    //    {
    //        if (obj is SpriteLibraryAsset spriteLib)
    //        {
    //            var data = Managers.Resource.Load<SpriteLibraryAsset>($"CharacterParts/{obj.name}");
    //            string[] categories = data.GetCategoryNames().ToArray();
    //            Sprite[] sprites = null;

    //            foreach (var category in categories)
    //            {
    //                sprites = null;

    //                // 레이블 이름을 저장
    //                string[] lbs = data.GetCategoryLabelNames(category).ToArray();

    //                sprites = new Sprite[lbs.Length];
    //                // Sprite를 모두 저장
    //                for (int k = 0; k < lbs.Length; k++)
    //                {
    //                    sprites[k] = data.GetSprite(category, lbs[k]);
    //                }

    //                // Asset의 특정 카테고리와 모든 레이블을 삭제
    //                for (int k = 0; k < lbs.Length; k++)
    //                {
    //                    spriteLib.RemoveCategoryLabel(category, lbs[k], false);
    //                }

    //                EditorUtility.SetDirty(spriteLib);
    //                AssetDatabase.SaveAssets();
    //                AssetDatabase.Refresh();

    //                for (int k = 0; k < lbs.Length; k++)
    //                {
    //                    spriteLib.AddCategoryLabel(sprites[k], category, $"{spriteLib.name}_{k}");

    //                }

    //                EditorUtility.SetDirty(spriteLib);
    //                AssetDatabase.SaveAssets();
    //                AssetDatabase.Refresh();
    //            }
    //        }
    //    }
    //}

    //[MenuItem("NPC/PartsToJson")]
    //static void PartsToJson()
    //{
    //    SpriteLibraryAsset[] assets = Resources.LoadAll<SpriteLibraryAsset>("CharacterParts");

    //    foreach (var asset in assets)
    //    {
    //        Dictionary<int, PartsLabelSprite> partDict = new Dictionary<int, PartsLabelSprite>();
    //        string[] categories = asset.GetCategoryNames().ToArray();

    //        for (int i = 0; i < categories.Length; i++)
    //        {
    //            Dictionary<string, Sprite> labelSprite = new Dictionary<string, Sprite>();
    //            string[] labels = null;

    //            labels = asset.GetCategoryLabelNames(categories[i]).ToArray();
    //            Sprite[] sprites = new Sprite[labels.Length];

    //            for (int j = 0; j < labels.Length; j++)
    //            {
    //                sprites[j] = asset.GetSprite(categories[i], labels[j]);
    //            }
    //            PartsLabelSprite pls = new PartsLabelSprite();
    //            pls.category = categories[i];
    //            pls.labels = labels;
    //            pls.sprites = sprites;

    //            partDict.Add(i, pls);
    //        }

    //        string data = partDict.ToJson(true);
    //        string path = $"{Application.dataPath}/Resources/Data/Json/{asset.name}.json";
    //        File.WriteAllText(path, data);
    //    }
    //}
    ////[MenuItem("NPC/Auto Seperate Sprite %&S")]
    ////static void AutoSeperateSprite()
    ////{
    ////    var objs = Selection.objects;

    ////    foreach(var obj in objs)
    ////    {
    ////        if(obj is Texture2D)
    ////        {
    ////            Texture2D spriteSheet = obj as Texture2D;
    ////            var factory = new SpriteDataProviderFactories();
    ////            factory.Init();

    ////            var dataProvider = factory.GetSpriteEditorDataProviderFromObject(spriteSheet);
    ////            dataProvider.InitSpriteEditorDataProvider();

    ////            // Texture 클릭 시 Inspector에 뜨는 설정창
    ////            var textureImporter = (dataProvider.targetObject as TextureImporter);

    ////            textureImporter.textureType = TextureImporterType.Sprite;
    ////            textureImporter.spritePixelsPerUnit = 16;
    ////            textureImporter.spriteImportMode = SpriteImportMode.Multiple;
    ////            textureImporter.filterMode = FilterMode.Point;
    ////            textureImporter.wrapMode = TextureWrapMode.Clamp;

    ////            textureImporter.SaveAndReimport();

    ////            TextureImporterSettings textureSettings = new TextureImporterSettings();

    ////            textureImporter.ReadTextureSettings(textureSettings);

    ////            textureSettings.spriteMeshType = SpriteMeshType.FullRect;
    ////            textureImporter.SetTextureSettings(textureSettings);

    ////            textureImporter.SaveAndReimport();

    ////            int sliceRange = 32;
    ////            int count = 0;
    ////            List<SpriteMetaData> newData = new List<SpriteMetaData>();

    ////            if (textureImporter.spriteImportMode == SpriteImportMode.Multiple)
    ////            {
    ////                for (int i = 0; i < spriteSheet.width; i += sliceRange)
    ////                {
    ////                    for(int j = spriteSheet.height; j > 0; j -= sliceRange)
    ////                    {
    ////                        SpriteMetaData meta = new SpriteMetaData();
    ////                        meta.pivot = new Vector2(0.5f, 0.5f);
    ////                        meta.alignment = (int)SpriteAlignment.Center;
    ////                        int rowNum = (spriteSheet.height - j) / sliceRange;
    ////                        int colNum = i / sliceRange;
    ////                        meta.name = $"{obj.name}_{count}";
    ////                        count++;
    ////                        meta.rect = new Rect(i, j - sliceRange, sliceRange, sliceRange);

    ////                        newData.Add(meta);
    ////                    }
    ////                }

    ////                textureImporter.spritesheet = newData.ToArray();

    ////                EditorUtility.SetDirty(textureImporter);
    ////                string path = AssetDatabase.GetAssetPath(obj);
    ////                AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

    ////                AssetDatabase.Refresh();
    ////            }
    ////        }
    ////    }
    ////}


    ////[MenuItem("NPC/Auto Seprate Top Parts Sprite")]
    ////static void AutoSeprateTopPartsSprite()
    ////{
    ////    SpriteLibraryAsset obj = Resources.Load<SpriteLibraryAsset>("CharacterParts/Test/Top");

    ////    if (obj is SpriteLibraryAsset)
    ////    {
    ////        SpriteLibraryAsset data = obj;

    ////        Sprite[] sprites = Resources.LoadAll<Sprite>("CharacterParts/Test/");
    ////        int count = 1;
    ////        int startIndex = 0;
    ////        int endIndex = 0;
    ////        var categoryNames = sprites[0].name.Split('_');

    ////        for (int k = 0; k < sprites.Length; k++)
    ////        {
    ////            string actionName = null;

    ////            if (k >= 0 * count && k <= 31 * count)
    ////            {
    ////                actionName = "Walk";
    ////                startIndex = 0 * count;
    ////                endIndex = 31 * count;
    ////            }
    ////            else if (k >= 32 * count && k <= 51 * count)
    ////            {
    ////                actionName = "Jump";
    ////                startIndex = 32 * count;
    ////                endIndex = 51 * count;
    ////            }
    ////            else if (k >= 52 * count && k <= 71 * count)
    ////            {
    ////                actionName = "Pickup";
    ////                startIndex = 52 * count;
    ////                endIndex = 71 * count;
    ////            }
    ////            else if (k >= 72 * count && k <= 103 * count)
    ////            {
    ////                actionName = "Carry";
    ////                startIndex = 72 * count;
    ////                endIndex = 103 * count;
    ////            }
    ////            else if (k >= 104 * count && k <= 119 * count)
    ////            {
    ////                actionName = "SwordSwing";
    ////                startIndex = 104 * count;
    ////                endIndex = 119 * count;
    ////            }
    ////            else if (k >= 120 * count && k <= 123 * count)
    ////            {
    ////                actionName = "Guard";
    ////                startIndex = 120 * count;
    ////                endIndex = 123 * count;
    ////            }
    ////            else if (k >= 124 * count && k <= 127 * count)
    ////            {
    ////                actionName = "Hurt";
    ////                startIndex = 124 * count;
    ////                endIndex = 127 * count;
    ////            }
    ////            else if (k >= 128 * count && k <= 129 * count)
    ////            {
    ////                actionName = "Die";
    ////                startIndex = 128 * count;
    ////                endIndex = 129 * count;
    ////            }
    ////            else if (k >= 130 * count && k <= 149 * count)
    ////            {
    ////                actionName = "PickAxe";
    ////                startIndex = 130 * count;
    ////                endIndex = 149 * count;
    ////            }
    ////            else if (k >= 150 * count && k <= 169 * count)
    ////            {
    ////                actionName = "Axe";
    ////                startIndex = 150 * count;
    ////                endIndex = 169 * count;
    ////            }
    ////            else if (k >= 170 * count && k <= 177 * count)
    ////            {
    ////                actionName = "Water";
    ////                startIndex = 170 * count;
    ////                endIndex = 177 * count;
    ////            }
    ////            else if (k >= 178 * count && k <= 197 * count)
    ////            {
    ////                actionName = "Hoe";
    ////                startIndex = 178 * count;
    ////                endIndex = 197 * count;
    ////            }
    ////            else if (k >= 198 * count && k <= 217 * count)
    ////            {
    ////                actionName = "Fishing";
    ////                startIndex = 198 * count;
    ////                endIndex = 217 * count;
    ////            }

    ////            int spriteIndex = k - startIndex;
    ////            string labelName = $"Top_{actionName}_{spriteIndex}";
    ////            data.AddCategoryLabel(sprites[k], $"{categoryNames[0]}_{categoryNames[1]}", labelName);

    ////            if (k % (217 * count) == 0 && k != 0)
    ////            {
    ////                count++;
    ////            }
    ////        }

    ////        EditorUtility.SetDirty(data);
    ////        AssetDatabase.SaveAssets();
    ////        AssetDatabase.Refresh();

    ////    }

    ////}
    //*/
    //#endregion
}

#endif