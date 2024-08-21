using Cinemachine;
using System.Threading.Tasks;
using UnityEngine;
public class GameScene : BaseScene
{
    public Player player;
    public Transform respawn;
    public CinemachineVirtualCamera _camera;

    public TimeManager timeManager => TimeManager.Instance;
    public GridManager gridManager => GridManager.Instance;

    private void OnEnable()
    {
        Managers.Game.onGameStart -= PlayGameSceneBGM;
        Managers.Game.onGameStart += PlayGameSceneBGM;
    }

    protected async override void Init()
    {
        base.Init();

        SceneType = Define.SceneType.GameScene;

        await CoWaitLoad();
    }

    async Task SpwanPlayerAsync()
    {
        // 플레이어 생성
        player = Instantiate(await ResourceManager.Instance.LoadAsset<Player>("player", eAddressableType.prefab));
        player.transform.position = respawn.position;
        player.GetComponent<AutoStyleSetting>()?.Init();
        _camera.Follow = player.transform;
    }

    private async Task CoWaitLoad()
    {
        while (!Managers.IsInitialized) await Task.Yield();
        while (!ResourceManager.Instance.isInit) await Task.Yield();

        await Managers.UI.ShowTaskPopupUI<UI_Fade>();

        Instantiate(await Managers.Resource.LoadAsset<GameObject>("WorldMap", eAddressableType.prefab));


        await Managers.Game.Init(); // 비동기 Init 호출
        await SpwanPlayerAsync();
        Canvas go = Instantiate(await Managers.Resource.LoadAsset<Canvas>("UI_TopLayerCanvas".ToLower(), eAddressableType.ui));
        go.transform.SetParent(Managers.UI.Root.transform);
        await Managers.UI.ShowSceneUI<UI_GameScene>();
        gridManager.Init();

        while (!GridManager.Instance.isInit) await Task.Yield();

        Managers.Resource.SetNormalNPC();
        //Managers.Resource.CreateMainNPC();

        timeManager.Init();

        while (!TimeManager.Instance.isInit) await Task.Yield();
        Managers.Game.GameStart();
        Managers.Resource.Instantiate("Tutorial");
    }
    public override void Clear()
    {
    }


    private void PlayGameSceneBGM() => SoundManager.Instance.Play(Define.Sound.Bgm, "BGM/BGM000", Managers.Sound.BgmVolume);
}
