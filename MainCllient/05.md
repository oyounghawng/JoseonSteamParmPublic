# Item
- 
    <aside>
    💡 **ItemObject를 활용한 아이템 관리**
    
    ---
    
    *📝 **모든 아이템에 ItemObject스크립트를 붙이고 거기에 데이터ItemObjectData 클래스를 선언하여 사용***
    
    ![image](https://github.com/user-attachments/assets/f08279a5-46f2-4e97-b414-cb78a8b747ff)

    
    📝 **ItemObjectData에 필요한 데이터 저장**
    
    ---
    
    - 각 타입별로 필요한 이미지 혹은 타일을 저장해두고 사용
    - TooltipUI 사용을 위해 StringBuilder를 반환해주는 메소드를 선언
    - 코드
        
        ```css
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
                        tooltipBuilder.Append(itemData.displayName);
                        break;
                }
        
                return tooltipBuilder.ToString();
            }
        
            private void AppendFarmTooltip(StringBuilder builder)
            {
                FarmData farmData = Managers.Data.FarmDatas[itemData.relationData];
                builder.AppendLine($"{itemData.displayName}");
                builder.Append(string.Join(", ", farmData.Season));
                builder.AppendLine("작물");
                builder.AppendLine($"이 작물이 자라기까지는 {farmData.GrowTime}일이 걸립니다.");
            }
        
            private void AppendFishingTooltip(StringBuilder builder)
            {
                FishData fishData = Managers.Data.FishDatas[itemData.relationData];
                builder.AppendLine($"{itemData.displayName}");
                builder.Append(string.Join(", ", fishData.Spot));
                builder.AppendLine("에서 잡을수 있습니다.");
                List<string> seasonNames = new List<string>();
                foreach (Season season in fishData.Season)
                {
                    seasonNames.Add(season.ToString());
                }
                builder.Append(string.Join(", ", seasonNames));
                builder.AppendLine("계절에 잡을수 있습니다. ");
            }
        }
        
        public class ItemObject : MonoBehaviour
        {
            public ItemObjectData itemObjectData;
        }
        ```
        
    </aside>
