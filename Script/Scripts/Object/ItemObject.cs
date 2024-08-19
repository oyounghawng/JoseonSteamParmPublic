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
        builder.AppendLine("�۹�");
        builder.AppendLine($"�� �۹��� �ڶ������� {farmData.GrowTime}���� �ɸ��ϴ�.");
        builder.AppendLine($"{itemData.description}");
    }

    private void AppendFishingTooltip(StringBuilder builder)
    {
        FishData fishData = Managers.Data.FishDatas[itemData.relationData];
        builder.AppendLine($"{itemData.displayName}");
        if(fishData.Spot.Equals("sea"))
        {
            builder.Append(string.Join(", ", "�ٴ�"));
        }
        else if (fishData.Spot.Equals("river"))
        {
            builder.Append(string.Join(", ", "��"));
        }
        builder.AppendLine("���� ������ �ֽ��ϴ�.");
        List<string> seasonNames = new List<string>();
        foreach (int season in fishData.Season)
        {
            string seasonName = season switch
            {
                1 => "��",
                2 => "����",
                3 => "����",
                4 => "�ܿ�",
                _ => "�� �� ����"
            };
            seasonNames.Add(seasonName);
        }
        builder.Append(string.Join(", ", seasonNames));
        builder.AppendLine("������ ������ �ֽ��ϴ�. ");
        builder.AppendLine($"{itemData.description}");
    }

    private string ConvertSeasonToKorean(string season)
    {
        return season switch
        {
            "Spring" => "��",
            "Summer" => "����",
            "Fall" => "����",
            "Winter" => "�ܿ�",
            _ => "�� �� ����"
        };
    }
}

public class ItemObject : MonoBehaviour
{
    public ItemObjectData itemObjectData;
}