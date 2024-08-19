using System;
using System.Collections.Generic;

[Serializable]
public class WorldTime
{
    public int Season;
    public int Day;

    public List<WeatherData> WeatherData;

    public WorldTime()
    {
        Season = 1;
        Day = 1;
        WeatherData = CreateWeatherData();
    }

    public void SetSeason(int Season)
    {
        this.Season = Season;
    }

    public void SetDay(int Day)
    {
        this.Day = Day;
    }

    public void SetWeatherData(List<WeatherData> WeatherData)
    {
        this.WeatherData = WeatherData;
    }

    private List<WeatherData> CreateWeatherData()
    {
        List<WeatherData> WeatherList = new List<WeatherData>();
        Dictionary<int, List<WeatherCreate>> dict = Managers.Data.WeatherCreateDict;
        foreach (var dt in dict)
        {
            foreach (var wt in dt.Value)
            {
                WeatherData wd = new WeatherData();
                wd.season = (Define.Season)wt.season;
                wd.day = wt.day;

                if (wt.probability >= 100)
                {
                    wd.weather = (Define.Weather)wt.weather;
                }
                else
                {
                    int pb = UnityEngine.Random.Range(0, 100);

                    if (pb > wt.probability)
                    {
                        wd.weather = Define.Weather.Sunny;
                    }
                    else
                    {
                        wd.weather = (Define.Weather)wt.weather;
                    }
                }
                WeatherList.Add(wd);
            }
        }
        return WeatherList;
    }

}
