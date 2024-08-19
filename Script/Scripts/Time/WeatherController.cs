using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[System.Serializable]
public class WeatherData
{
    public Define.Season season;

    public int day;

    public Define.Weather weather;

    public WeatherData() { }
}

public class WeatherController
{
    // 날씨에 따른 파티클 저장소
    Dictionary<string, ParticleSystem> weatherDic = new Dictionary<string, ParticleSystem>();

    // 날씨 기록
    List<WeatherData> WeatherData = null;
    private GameObject weatherObj = null;

    public Define.Weather Weather { get; private set; } = Define.Weather.Sunny;

    public WeatherController(List<WeatherData> WeatherData)
    {
        this.WeatherData = WeatherData;
    }

    public void Init()
    {
        // 파티클 저장

        List<ParticleSystem> weather = Managers.Resource.LoadAll<ParticleSystem>("Prefabs/Weather").ToList();
        foreach (var wd in weather)
        {
            if (!weatherDic.ContainsKey(wd.name))
                weatherDic.Add(wd.name, wd);
        }
    }

    public void SetWeather(int season, int day)
    {
        Weather = WeatherData.Find(x => (x.season == (Define.Season)season) && (x.day == day)).weather;

        switch (Weather)
        {
            case Define.Weather.Sunny:
                if (weatherObj != null)
                    Managers.Resource?.Destroy(weatherObj);
                break;
            default:
                if (weatherObj != null)
                    Managers.Resource?.Destroy(weatherObj);
                string weatherName = Enum.GetName(typeof(Define.Weather), Weather);
                weatherObj = Managers.Resource?.Instantiate(weatherDic[weatherName].gameObject);
                break;
        }
    }
}
