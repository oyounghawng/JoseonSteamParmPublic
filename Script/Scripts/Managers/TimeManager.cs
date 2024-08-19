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

        // 월드 시간 정보를 가져옵니다. 로드 데이터가 있을 경우 해당 정보를 가져옵니다.
        WorldTime = Managers.Game.SaveData.WorldTime;

        Day = WorldTime.Day;
        Season = WorldTime.Season;

        // 날씨 초기화
        weatherController = new WeatherController(WorldTime.WeatherData);
        weatherController.Init();

        weatherController.SetWeather(Season, Day);
        onChangedWeather?.Invoke((int)weatherController.Weather);


        // 시간, 일, 계절, 날씨 데이터 초기화
        hour = 6;
        Minute = 0;

        isInit = true;
    }

    private void OnEnable()
    {
        Managers.Game.onGameStart += StartTimer;
    }

    // 하루가 지났음을 알리는 메서드
    // TODO : FadeIn -> 결산 -> 날짜 바뀜 -> 저장 (저장중 + 넘어가는 것을 보여줌) -> 다시 시작 
    public async void EndDay()
    {
        isEnd = true;
        onEndDay?.Invoke();

        // TODO : 브금 끄기
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

    // 시간을 카운팅 해주는 메서드
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

        // Season 초기화
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