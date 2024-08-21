# Farm
- 
    <aside>
    💡 **GridManager에서 모든 농사시스템을 관리**
    
    ---
    
    *📝 ***GridManager의 TileRowClass 선언**
    
    - 게임 시작 시 아이템에 있는 타일 정보를 가져와서 Dictionary에 저장하는 방식으로 효율적으로 모든 농사용 타일들을 관리
    - 코드
        
        ```css
        [System.Serializable]
        public class TileRow
        {
            public List<TileBase> tiles;
        
            public TileRow(List<TileBase> tiles)
            {
                this.tiles = tiles;
            }
        }
        
        ```
        
    
    📝 **GridManager의 메소드를 통해 타일의 상태 관리**
    
    ---
    
    - 농사를 하기위해 하는 행동들에 따라 타일의 상태가 변화
    - 타일의 모든 상태를 관리하고 변화하는 메소드를 선언 후 사용
    - 코드
        
        ```css
            public bool IsInteractable(Vector3Int position)
            {
                TileBase tile = interactableMap.GetTile(position);
                return tile != null && tile.name.Equals("Interactable");
            }
        
            public void PlowedTile(Vector3Int position)
            {
                if (weather == Define.Weather.Rainy || weather == Define.Weather.ThunderStorm)
                {
                    interactableMap.SetTile(position, wetTile);
                    wetTileDays[position] = 0; // wetTile로 변경된 경우 wetTileDays 초기화
                }
                else
                {
                    interactableMap.SetTile(position, plowedTile);
                }
            }
        
            public void WetTile(Vector3Int position)
            {
                interactableMap.SetTile(position, wetTile);
                wetTileDays[position] = 0;
            }
        
            private void OnChangedWeather(int weather)
            {
                this.weather = (Define.Weather)weather;
        
                List<Vector3Int> wetPositions = new List<Vector3Int>(wetTileDays.Keys);
                foreach (Vector3Int position in wetPositions)
                {
                    if ((Define.Weather)weather == Define.Weather.Rainy || (Define.Weather)weather == Define.Weather.ThunderStorm)
                    {
                        interactableMap.SwapTile(plowedTile, wetTile);
                        wetTileDays[position] = 0;
                    }
                    else
                    {
                        interactableMap.SwapTile(wetTile, plowedTile);
                    }
                }
            }
        
            public void Plant(Vector3Int position, ItemObjectData itemObjectData)
            {
                if (!plantedCrops.ContainsKey(position))
                {
                    plantedCrops[position] = itemObjectData.itemData.relationData; //심어진 작물 dictionary에 추가
                    growthDays[position] = 1;
                    wetTileDays[position] = 0;
                    UpdateCropTile(position, itemObjectData.itemData.relationData, 1);
                    (Managers.Scene.CurrentScene as GameScene).player.RemoveItem(itemObjectData);
                }
            }
        
            private void UpdateCropTile(Vector3Int position, string cropCode, int days)
            {
                cropTiles.TryGetValue(cropCode, out TileRow row);
                if (days < row.tiles.Count)
                {
                    plowedMap.SetTile(position, row.tiles[days]);
                }
                else
                {
                    plowedMap.SetTile(position, row.tiles[0]);
                }
            }
        
            public async void Havest(Vector3Int position)
            {
                if (plantedCrops.ContainsKey(position))
                {
                    string cropCode = plantedCrops[position];
                    FarmData crop;
                    DataManager.Instance.FarmDatas.TryGetValue(cropCode, out crop);
                    int currentIndex = growthDays[position];
        
                    if (currentIndex < crop.GrowTime + 1)
                    {
                        Debug.Log("수확불가");
                        return;
                    }
        
                    Debug.Log("수확");
        
                    GameObject go = Instantiate(await ResourceManager.Instance.LoadAsset<GameObject>(crop.RewardItem.ToLower(), eAddressableType.prefab));
                    go.transform.position = position;
        
                    plowedMap.SetTile(position, null);  //타일 비우기
                    plantedCrops.Remove(position);  //심어진 작물 dictionary에서 제거
                    wetTileDays.Remove(position);  //젖은 타일 dictionary에서 제거
                    growthDays.Remove(position);
                }
            }
        ```
        
    </aside>
