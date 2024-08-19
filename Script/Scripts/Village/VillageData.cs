using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class VillageData
{
    // 인구 수
    public int population;
    public int maxPopulation;
    // 평판도
    public int reputation;
    // 레벨
    public int level;

    public List<NPCData> npcData;

    public List<BuildingInfo> buildingInfo;
    public VillageData()
    {
        population = 1;
        reputation = 0;
        level = 1;
        maxPopulation = 4;
    }

    public void Init()
    {
        npcData = new List<NPCData>();
        buildingInfo = new List<BuildingInfo>();
        buildingInfo = Managers.Data.BuildingInfoDict.Values.ToList();

    }

    public void AddPopulation(int population)
    {
        if (maxPopulation > population)
            this.population += population;
    }
    public void SetLevel(int level)
    {
        switch (level)
        {
            case 1:
                maxPopulation = 4;
                break;
            case 2:
                maxPopulation = 8;
                break;
            case 3:
                maxPopulation = 12;
                break;
            case 4:
                maxPopulation = 16;
                break;
            case 5:
                maxPopulation = 20;
                break;
        }
    }

}
