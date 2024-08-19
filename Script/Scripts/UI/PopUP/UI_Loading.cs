using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class UI_Loading : UI_Popup
{
    [SerializeField]
    private Slider loading;

    static string nextScene;

    public void SetProgress(float progress)
    {
        loading.value = progress;
    }
    public static void LoadScene(string sceneName)
    {
        nextScene = sceneName;
        SceneManager.LoadScene("LodingScsne");
    }

    //private void Start()
    //{
    //    StartCoroutine(LoadSceneProcess());
    //}

    //IEnumerator LoadSceneProcess()
    //{
    //    AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
    //    op.allowSceneActivation = false;

    //    float timer = 0f;
    //    while (!op.isDone)
    //    {
    //        yield return null;

    //        if (op.progress < 0.9f)
    //        {
    //            loading.value = op.progress;
    //        }
    //        else
    //        {
    //            timer += Time.unscaledDeltaTime;
    //            loading.value = Mathf.Lerp(0.9f, 1f, timer);
    //            if (loading.value >= 1f)
    //            {
    //                op.allowSceneActivation = true;
    //                yield break;
    //            }
    //        }
    //    }
    //}

    public override void Init()
    {
        base.Init();
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }
}
