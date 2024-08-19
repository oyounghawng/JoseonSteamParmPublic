using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx : Singleton<SceneManagerEx>
{
    public override void Init()
    {

    }

    private Define.SceneType _curSceneType = Define.SceneType.Unknown;

    public Define.SceneType CurrentSceneType
    {
        get
        {
            if (_curSceneType != Define.SceneType.Unknown)
                return _curSceneType;
            return CurrentScene.SceneType;
        }
        set { _curSceneType = value; }
    }
    public BaseScene CurrentScene { get { return GameObject.FindObjectOfType<BaseScene>(); } }

    public void LoadScene(Define.SceneType type)
    {
        Managers.Clear();

        _curSceneType = type;
        SceneManager.LoadScene(GetSceneName(type));
    }

    public async Task LoadSceneAsync(Define.SceneType NextScene)
    {
        UI_Loading loading = null;
        var loadingOperation = SceneManager.LoadSceneAsync(GetSceneName(Define.SceneType.LoadingScene), LoadSceneMode.Additive);

        await loadingOperation.AsTask();

        await SceneManager.UnloadSceneAsync(GetSceneName(Define.SceneType.LogoScene)).AsTask();

        loading = await Managers.UI.ShowTaskPopupUI<UI_Loading>();

        await Managers.Game.SaveGameAsync();
        
        var operation = SceneManager.LoadSceneAsync(GetSceneName(NextScene), LoadSceneMode.Single);

        operation.allowSceneActivation = false;


        while (operation.progress < 0.9f)
        {
            loading.SetProgress(operation.progress);
            await Task.Yield();
        }
        loading.SetProgress(1);

        await Task.Delay(TimeSpan.FromSeconds(1));
        Managers.UI.ClosePopupUI(loading);

        operation.allowSceneActivation = true;


        await operation.AsTask();
    }


    public async void UnLoadSceneAsync(Define.SceneType sceneType)
    {
        await SceneManager.UnloadSceneAsync(GetSceneName(sceneType)).AsTask();
    }
    string GetSceneName(Define.SceneType type)
    {
        string name = System.Enum.GetName(typeof(Define.SceneType), type);
        char[] letters = name.ToLower().ToCharArray();
        letters[0] = char.ToUpper(letters[0]);
        return new string(letters);
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
