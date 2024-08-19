using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Define;

[System.Serializable]
public class ItemObjectData
{
    public string itemKey;
    public ItemData itemData = new ItemData();
    public int count;

    [Header("Images")]
    public Sprite image;
    public Sprite silluetImage;

    [Header("CropsTiles")]
    public List<TileBase> tiles;

    public ItemObjectData(ItemObjectData itemObjectData)
    {
        this.itemKey = itemObjectData.itemKey;
        this.itemData = itemObjectData.itemData;
        this.count = 1;
        this.image = itemObjectData.image;
    }

    public string GetTooltip()
    {
        StringBuilder tooltipBuilder = new StringBuilder();

        switch (itemData.type)
        {
            case (int)ItemType.FarmItem:
                AppendFarmTooltip(tooltipBuilder);
                break;
            case (int)ItemType.FishingItem:
                AppendFishingTooltip(tooltipBuilder);
                break;
            default:
                tooltipBuilder.AppendLine(itemData.displayName);
                tooltipBuilder.AppendLine(itemData.description);
                break;
        }

        return tooltipBuilder.ToString();
    }

    private void AppendFarmTooltip(StringBuilder builder)
    {
        FarmData farmData = Managers.Data.FarmDatas[itemData.relationData];
        builder.AppendLine($"{itemData.displayName}");
        builder.Append(string.Join(", ", ConvertSeasonToKorean(farmData.Season)));
        builder.AppendLine("작물");
        builder.AppendLine($"이 작물이 자라기까지는 {farmData.GrowTime}일이 걸립니다.");
        builder.AppendLine($"{itemData.description}");
    }

    private void AppendFishingTooltip(StringBuilder builder)
    {
        FishData fishData = Managers.Data.FishDatas[itemData.relationData];
        builder.AppendLine($"{itemData.displayName}");
        if(fishData.Spot.Equals("sea"))
        {
            builder.Append(string.Join(", ", "바다"));
        }
        else if (fishData.Spot.Equals("river"))
        {
            builder.Append(string.Join(", ", "강"));
        }
        builder.AppendLine("에서 잡을수 있습니다.");
        List<string> seasonNames = new List<string>();
        foreach (int season in fishData.Season)
        {
            string seasonName = season switch
            {
                1 => "봄",
                2 => "여름",
                3 => "가을",
                4 => "겨울",
                _ => "알 수 없음"
            };
            seasonNames.Add(seasonName);
        }
        builder.Append(string.Join(", ", seasonNames));
        builder.AppendLine("계절에 잡을수 있습니다. ");
        builder.AppendLine($"{itemData.description}");
    }

    private string ConvertSeasonToKorean(string season)
    {
        return season switch
        {
            "Spring" => "봄",
            "Summer" => "여름",
            "Fall" => "가을",
            "Winter" => "겨울",
            _ => "알 수 없음"
        };
    }
}

public class ItemObject : MonoBehaviour
{
    public ItemObjectData itemObjectData;
}