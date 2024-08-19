using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();
        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }
    public static Color SetColorAlpha(Image _image, float _alpha)
    {
        Color color = _image.color;
        color.a = _alpha;
        return color;
    }

    public static string GetBuildTargetToString()
    {
#if UNITY_EDITOR
        return EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android ? "Android" : EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS ? "IOS" : "StandaloneWindows";
#endif
        return Application.platform == RuntimePlatform.Android ? "Android" : Application.platform == RuntimePlatform.IPhonePlayer ? "IOS" : "StandaloneWindows";
    }
}
