using System;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class GameData
{
    public string playerName;
    public string favoriteThing;
    public string villageName;

    public CharacterLooks Look;
    public WorldTime WorldTime;
    public VillageData villageData;
    private int gold;

    public bool BGMOn;
    public bool EffectSoundOn;

    public GameData()
    {
        playerName = "";
        favoriteThing = "";
        villageName = "";
        gold = 1000;
        Look = new CharacterLooks();
        WorldTime = new WorldTime();
        villageData = new VillageData();
        BGMOn = true;
        EffectSoundOn = true;
    }

    public int Gold
    {
        get
        {
            return gold;
        }
        set
        {
            gold += value;
            (SceneManagerEx.Instance.CurrentScene as GameScene).player.resetPlayerEquipItem?.Invoke();
        }
    }

    public void SetLooks(CharacterLooks looks)
    {
        this.Look = looks;
    }

    public void SetName(string name)
    {
        playerName = name;
    }

    public void SetFavoriteThing(string favoriteThing)
    {
        this.favoriteThing = favoriteThing;
    }

}

public class GameManagerEx : Singleton<GameManagerEx>
{
    GameData _gameData;
    public GameData SaveData
    {
        get
        {
            return _gameData;
        }
        set
        {
            _gameData = value;
        }
    }

    [NonSerialized]
    public bool isInit = false;
    public bool IsLoaded = false;

    public bool isGameStart = false;

    public event Action onGameStart = null;

    public Action<string> onChangedPlayerLocation = null;

    public string playerLocation = string.Empty;
    
    public async Task Init()
    {
        _path = Application.persistentDataPath + "/SaveData.json";

        if (await LoadGameAsync())
        {
            isInit = true;
            IsLoaded = true;
            return;
        }

        SaveData = new GameData();
        SaveData.villageData.Init();
        await SaveGameAsync();
        isInit = true;

        onChangedPlayerLocation -= SetPlayerLocation;
        onChangedPlayerLocation += SetPlayerLocation;
    }


    //public override void Init()
    //{
    //    _path = Application.persistentDataPath + "/SaveData.json";
    //    if (LoadGame())
    //    {
    //        isInit = true;
    //        IsLoaded = true;
    //        return;
    //    }

    //    SaveData = new GameData();
    //    SaveData.villageData.Init();
    //    SaveGame();
    //    isInit = true;
    //}


    #region Option

    public bool BGMOn
    {
        get { return _gameData.BGMOn; }
        set { _gameData.BGMOn = value; }
    }

    public bool EffectSoundOn
    {
        get { return _gameData.EffectSoundOn; }
        set { _gameData.EffectSoundOn = value; }
    }
    #endregion


    #region Save&Load
    string _path;

    public void SaveGame()
    {
        string jsonStr = JsonUtility.ToJson(SaveData, true);
        File.WriteAllText(_path, jsonStr);
    }

    public async Task SaveGameAsync()
    {
        string jsonStr = JsonUtility.ToJson(SaveData, true);
        await File.WriteAllTextAsync(_path, jsonStr);
    }

    public bool LoadGame()
    {
        if (File.Exists(_path) == false)
            return false;

        string fileStr = File.ReadAllText(_path);
        GameData data = JsonUtility.FromJson<GameData>(fileStr);

        if (data != null)
            SaveData = data;
        else
            Debug.Log("직렬화 실패");

        IsLoaded = true;
        return true;
    }

    public async Task<bool> LoadGameAsync()
    {
        if (!File.Exists(_path))
            return false;

        string fileStr = await File.ReadAllTextAsync(_path);
        GameData data = JsonUtility.FromJson<GameData>(fileStr);

        if (data != null)
            SaveData = data;
        else
            Debug.Log("직렬화 실패");

        IsLoaded = true;
        return true;
    }

    #endregion

    public void GameStart()
    {
#if UNITY_EDITOR
        Debug.Log("게임 시작");
#endif
        isGameStart = true;
        onGameStart?.Invoke();
    }

    private void SetPlayerLocation(string place)
    {
        playerLocation = place;
    }
}
