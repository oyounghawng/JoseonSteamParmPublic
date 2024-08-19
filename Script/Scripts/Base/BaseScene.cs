using UnityEngine;
using UnityEngine.EventSystems;

public abstract class BaseScene : MonoBehaviour
{
    public Define.SceneType SceneType { get; protected set; } = Define.SceneType.Unknown;
    public bool IsInitialized { get; protected set; } = false;

    void Awake()
    {
        Init();
    }

    protected virtual void Init()
    {
        Object obj = GameObject.FindObjectOfType(typeof(EventSystem));
        /*
        if (obj == null)
            Managers.Resource.Instantiate("UI/EventSystem").name = "@EventSystem";
        */
    }

    public abstract void Clear();
}
