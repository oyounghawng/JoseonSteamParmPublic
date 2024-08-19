using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class TimeManager : MonoSingleton<TimeManager>
{
    public WorldTime WorldTime { get; private set; }

    private Coroutine coStarTime = null;

    private float timeInterval = 4.0f;

    #region Second, Minute, Hour, Day, Month, Weather, Season

    [Range(1, 4)]
    private int season = 1;

    [Range(1, 28)]
    private int day = -1;

    public int Day
    {
        get => day;
        set
        {
            day = value;
            onChangedDay?.Invoke(day);
        }
    }
    [Range(6, 24)]
    private int hour = 6;

    [Range(0, 60)]
    private int minute = -1;

    public int Minute
    {
        get => minute;
        set
        {
            minute = value;

            int allTime = (hour * 60) + minute;
            onChangedTime?.Invoke(allTime);
        }
    }

    public int Season
    {
        get => season;
        set
        {
            season = value;
            onChangedSeason?.Invoke(season);
        }
    }

    WeatherController weatherController;

    public bool isInit = false;

    bool isEnd = false;

    public event Action<int> onChangedTime;
    public event Action<int> onChangedDay;
    public event Action<int> onChangedSeason;
    public event Action<int> onChangedWeather;

    public event Action onStartDay = null;
    public event Action onEndDay = null;

    #endregion
    public override void Init()
    {
        base.Init();

        // ���� �ð� ������ �����ɴϴ�. �ε� �����Ͱ� ���� ��� �ش� ������ �����ɴϴ�.
        WorldTime = Managers.Game.SaveData.WorldTime;

        Day = WorldTime.Day;
        Season = WorldTime.Season;

        // ���� �ʱ�ȭ
        weatherController = new WeatherController(WorldTime.WeatherData);
        weatherController.Init();

        weatherController.SetWeather(Season, Day);
        onChangedWeather?.Invoke((int)weatherController.Weather);


        // �ð�, ��, ����, ���� ������ �ʱ�ȭ
        hour = 6;
        Minute = 0;

        isInit = true;
    }

    private void OnEnable()
    {
        Managers.Game.onGameStart += StartTimer;
    }

    // �Ϸ簡 �������� �˸��� �޼���
    // TODO : FadeIn -> ��� -> ��¥ �ٲ� -> ���� (������ + �Ѿ�� ���� ������) -> �ٽ� ���� 
    public async void EndDay()
    {
        isEnd = true;
        onEndDay?.Invoke();

        // TODO : ��� ����
        Managers.Sound.Stop(Define.Sound.Bgm);

        await SettleDay();
    }

    public void StartTimer()
    {
        isEnd = false;

        onChangedWeather?.Invoke((int)weatherController.Weather);

        onStartDay?.Invoke();

        if (coStarTime != null)
        {
            StopCoroutine(coStarTime);
        }
        coStarTime = StartCoroutine(Timer());
    }

    public void StopTimer()
    {
        if (coStarTime != null)
        {
            StopCoroutine(coStarTime);
        }
    }

    // �ð��� ī���� ���ִ� �޼���
    private IEnumerator Timer()
    {
        while (!isEnd)
        {
            yield return CoroutineHelper.WaitForSeconds(timeInterval);
            if (minute >= 50)
            {
                hour++;
                Minute = 0;
                if (hour >= 24)
                {
                    EndDay();
                    yield break;
                }
            }
            else
            {
                Minute += 10;

            }
        }
        yield break;
    }

    private async Task SettleDay()
    {
        UI_Fade fade = await Managers.UI.ShowTaskPopupUI<UI_Fade>();

        await fade.FadeIn();
        UI_SettleAccount result = await Managers.UI.ShowTaskPopupUI<UI_SettleAccount>();
        var saveData = Managers.Game.SaveData;

        result.StartSettleDay(saveData.Gold, saveData.villageData.population, saveData.villageData.reputation, saveData.villageData.level);

        while (!result.isFinished) await Task.Yield();
        await Managers.Game.SaveGameAsync();

        NextDay();
        StartTimer();
        Managers.Sound.Play(Define.Sound.Bgm);
        await fade.FadeOut();
        Managers.UI.CloseAllPopupUI();
    }

    private void NextDay()
    {
        hour = 6;
        Minute = 0;
        Day++;

        // Season �ʱ�ȭ
        if (Day > 28)
        {
            Day = 1;
            Season++;

            if (Season >= 5)
                Season = 1;

            onChangedSeason?.Invoke(Season);
        }

        weatherController.SetWeather(Season, Day);
        onChangedWeather?.Invoke((int)weatherController.Weather);
    }

}