using System.Collections;
using UnityEngine;

public class LogoScene : BaseScene
{
    protected override void Init()
    {
        base.Init();

        SceneType = Define.SceneType.LogoScene;
        StartCoroutine(CoWaitLoad());
    }

    IEnumerator CoWaitLoad()
    {
        yield return new WaitUntil(() => ResourceManager.Instance.isInit);
    }

    public void OpenCharacter()
    {
        SpwanAsync();
    }
    async void SpwanAsync()
    {
        await UIManager.Instance.ShowTaskPopupUI<UI_CharacterCustom>();
    }
    public override void Clear()
    {
    }
}