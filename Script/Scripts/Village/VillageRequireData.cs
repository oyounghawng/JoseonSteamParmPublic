using System;
using System.Collections.Generic;

[Serializable]
public class VillageMonthlyRequiredData
{
    public int season;
    public List<VillageRequireData> requiredDatas;
}

[Serializable]
public class VillageRequireData
{
    public ItemObjectData requiredObjectData;

    public int requiredCount;

    public int requiredGold;

    public VillageRequireData() { }

    public VillageRequireData(ItemObjectData requiredObjectData, int requiredCount, int requiredGold)
    {
        this.requiredObjectData = requiredObjectData;
        this.requiredCount = requiredCount;
        this.requiredGold = requiredGold;
    }
}
