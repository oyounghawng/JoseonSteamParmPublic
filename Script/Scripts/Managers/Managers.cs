using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers s_instance = null;
    static Managers Instance { get { Init(); return s_instance; } }

    public static bool IsInitialized { get; private set; } = false;

    public static DataManager Data => DataManager.Instance;
    public static GameManagerEx Game => GameManagerEx.Instance;
    public static PoolManager Pool => PoolManager.Instance;
    public static ResourceManager Resource => ResourceManager.Instance;
    public static SceneManagerEx Scene => SceneManagerEx.Instance;
    public static SoundManager Sound => SoundManager.Instance;
    public static UIManager UI => UIManager.Instance;

    void Awake()
    {
        if (s_instance == null)
            Init();
        else
            Destroy(this.gameObject);
    }

    static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_instance = go.GetComponent<Managers>();

            DataManager.Instance.Init();
            ResourceManager.Instance.Init();
            PoolManager.Instance.Init();
            SceneManagerEx.Instance.Init();
            SoundManager.Instance.Init();
            UIManager.Instance.Init();

            IsInitialized = true;
        }
    }
    public static void Clear()
    {
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }
}
