using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class TileRow
{
    public List<TileBase> tiles;

    public TileRow(List<TileBase> tiles)
    {
        this.tiles = tiles;
    }
}

public class GridManager : MonoSingleton<GridManager>
{
    [NonSerialized]
    public bool isInit = false;

    [Header("이동 가능한 영역")]
    private Tilemap walkableTileMap;
    private Tilemap resourceTileMap;

    public Tilemap plowedMap;
    public Tilemap interactableMap;

    public Tile hiddenInteractableTile;
    public Tile plowedTile;
    public Tile wetTile;

    private Dictionary<string, TileRow> cropTiles = new Dictionary<string, TileRow>();
    private Dictionary<Vector3Int, string> plantedCrops = new Dictionary<Vector3Int, string>();
    private Dictionary<Vector3Int, int> wetTileDays = new Dictionary<Vector3Int, int>();
    private Dictionary<Vector3Int, int> growthDays = new Dictionary<Vector3Int, int>();

    private Define.Weather weather;

    [Header("대각선 여부 판단")]
    [SerializeField] bool diagonal;
    private AStarNode[,] grid;

    private void OnEnable()
    {
        TimeManager.Instance.onEndDay -= OnEndDay;
        TimeManager.Instance.onEndDay += OnEndDay;

        TimeManager.Instance.onChangedWeather -= OnChangedWeather;
        TimeManager.Instance.onChangedWeather += OnChangedWeather;
    }

    public async override void Init()
    {
        diagonal = false;
        GameObject.Find("@Walkable").TryGetComponent(out walkableTileMap);
        //TileMap
        plowedMap = GameObject.Find("PlowedMap").GetComponent<Tilemap>();
        interactableMap = GameObject.Find("InteractableMap").GetComponent<Tilemap>();
        resourceTileMap = GameObject.Find("Resource").GetComponent<Tilemap>();

        //Tile
        hiddenInteractableTile = await ResourceManager.Instance.LoadAsset<Tile>("interactable", eAddressableType.tile);
        plowedTile = await ResourceManager.Instance.LoadAsset<Tile>("plowed", eAddressableType.tile);
        wetTile = await ResourceManager.Instance.LoadAsset<Tile>("wet", eAddressableType.tile);

        foreach (var position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);

            if (tile != null && tile.name.Equals("Interactable_Visible"))
            {
                interactableMap.SetTile(position, hiddenInteractableTile);
            }
        }

        List<ItemData> itemDatas = DataManager.Instance.ItemDatas.Values.Where(data => data.displayName.Contains("씨앗")).ToList();
        foreach (ItemData data in itemDatas)
        {
            GameObject go = await ResourceManager.Instance.LoadAsset<GameObject>(data.rCode.ToLower(), eAddressableType.prefab);
            if (go == null)
                continue;

            ItemObjectData itemObjectdata = go.GetComponent<ItemObject>().itemObjectData;
            List<TileBase> tiles = itemObjectdata.tiles;
            TileRow row = new TileRow(tiles);
            cropTiles.Add(itemObjectdata.itemData.relationData, row);
        }

        CreateGrid();
        isInit = true;
    }

    public void Clear()
    {
        grid.Clear();
    }

    #region Tilemap Method

    public Vector3Int WorldToCell(Vector3 pos, Define.TilemapType type)
    {
        if (type == Define.TilemapType.WalkableTileMap)
        {
            return walkableTileMap.WorldToCell(pos);
        }
        else if (type == Define.TilemapType.InteractableMap)
        {
            return interactableMap.WorldToCell(pos);
        }
        else if (type == Define.TilemapType.WaterInteractableMap)
        {
            return Vector3Int.zero;
        }
        return Vector3Int.zero;
    }
    public Vector3 WorldToCell(Vector3Int pos, Define.TilemapType type)
    {
        if (type == Define.TilemapType.WalkableTileMap)
        {
            return walkableTileMap.CellToWorld(pos);
        }
        else if (type == Define.TilemapType.InteractableMap)
        {
            return interactableMap.CellToWorld(pos);
        }
        else if (type == Define.TilemapType.WaterInteractableMap)
        {
            return Vector3.zero;
        }
        return Vector3.zero;
    }


    #endregion


    #region PathFinder
    private void CreateGrid()
    {
        if (walkableTileMap != null && resourceTileMap != null && plowedMap != null)
        {
            walkableTileMap.CompressBounds();

            BoundsInt walkableBounds = walkableTileMap.cellBounds;

            grid = new AStarNode[walkableBounds.size.y, walkableBounds.size.x];

            for (int y = walkableBounds.yMin, i = 0; i < walkableBounds.size.y; y++, i++)
            {
                for (int x = walkableBounds.xMin, j = 0; j < walkableBounds.size.x; x++, j++)
                {
                    AStarNode node = new AStarNode();
                    node.xIndex = j;
                    node.yIndex = i;
                    node.gCost = int.MaxValue;
                    node.parent = null;
                    node.xPos = walkableTileMap.CellToWorld(new Vector3Int(x, y)).x;
                    node.yPos = walkableTileMap.CellToWorld(new Vector3Int(x, y)).y;

                    if (walkableTileMap.HasTile(new Vector3Int(x, y, 0)))
                    {
                        node.isWalkable = true;
                        grid[i, j] = node;
                    }
                    else
                    {
                        node.isWalkable = false;
                        grid[i, j] = node;
                    }
                }
            }
        }

    }

    public void ResetNode()
    {
        foreach (var node in grid)
        {
            node.Reset();
        }
    }

    // 월드좌표를 통해서 Grid 내의 AStarNode를 반환
    public AStarNode GetNodeFromWorld(Vector3 worldPosition)
    {
        Vector3Int cellPos = walkableTileMap.WorldToCell(worldPosition);

        int y = cellPos.y - walkableTileMap.cellBounds.yMin;
        int x = cellPos.x - walkableTileMap.cellBounds.xMin;

        AStarNode node = null;
        if (grid != null)
            node = grid[y, x];

        return node;
    }

    public Vector3 GetWorldFromNode(AStarNode node)
    {
        return new Vector3(node.xPos, node.yPos, 0);
    }
    public AStarNode GetRandomNode()
    {
        int y = UnityEngine.Random.Range(0, grid.GetLength(0));
        int x = UnityEngine.Random.Range(0, grid.GetLength(1));

        if (grid[y, x] != null)
            return grid[y, x];

        return null;
    }

    public List<AStarNode> GetNeighborNodes(AStarNode node, bool diagonal = false)
    {
        List<AStarNode> neighbors = new List<AStarNode>();

        int height = grid.GetLength(0) - 1;
        int width = grid.GetLength(1) - 1;

        int y = node.yIndex;
        int x = node.xIndex;

        if (y < height)
            neighbors.Add(grid[y + 1, x]);
        if (y > 0)
            neighbors.Add(grid[y - 1, x]);

        if (x < width)
            neighbors.Add(grid[y, x + 1]);
        if (x > 0)
            neighbors.Add(grid[y, x - 1]);

        if (!diagonal) return neighbors;

        if (x > 0 && y > 0)
            neighbors.Add(grid[y - 1, x - 1]);
        if (x < width && y > 0)
            neighbors.Add(grid[y - 1, x + 1]);
        if (x < width && y < height)
            neighbors.Add(grid[y + 1, x + 1]);
        if (x > 0 && y < height)
            neighbors.Add(grid[y + 1, x - 1]);

        return neighbors;
    }


    public Dictionary<int, Vector3> GetPath(Vector3 start, Vector3 end)
    {
        AStarNode startNode = new AStarNode();
        AStarNode endNode = new AStarNode();

        startNode = GetNodeFromWorld(start);
        endNode = GetNodeFromWorld(end);


        List<AStarNode> path = ManagerLocator.GetService<AStarPathFinder>().CreatePath(startNode, endNode, diagonal);
        Dictionary<int, Vector3> destDict = new Dictionary<int, Vector3>();
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3Int endCellPos = walkableTileMap.WorldToCell(new Vector3(path[i + 1].xPos, path[i + 1].yPos));
                Vector3 endCenterPos = walkableTileMap.GetCellCenterWorld(endCellPos);
                endCenterPos -= walkableTileMap.cellGap / 2;

#if UNITY_EDITOR
                Debug.DrawLine(new Vector3(path[i].xPos, path[i].yPos), new Vector3(path[i + 1].xPos, path[i + 1].yPos), Color.black, 2f);
#endif
                destDict.Add(i, endCenterPos);
            }
        }

        return destDict;
    }


    #endregion


    #region FarmManager
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
                return;
            }
            GameObject go = Instantiate(await ResourceManager.Instance.LoadAsset<GameObject>(crop.RewardItem.ToLower(), eAddressableType.prefab));
            go.transform.position = position;

            interactableMap.SetTile(position, hiddenInteractableTile);
            plowedMap.SetTile(position, null);  //타일 비우기
            plantedCrops.Remove(position);  //심어진 작물 dictionary에서 제거
            wetTileDays.Remove(position);  //젖은 타일 dictionary에서 제거
            growthDays.Remove(position);
        }
        else
        {
            interactableMap.SetTile(position, hiddenInteractableTile);
        }
    }

    private void OnEndDay()
    {
        //하루 지나면 작물 자라기
        List<Vector3Int> positionsToUpdate = new List<Vector3Int>(plantedCrops.Keys);
        foreach (Vector3Int position in positionsToUpdate)
        {
            string cropCode = plantedCrops[position];
            FarmData crop;
            DataManager.Instance.FarmDatas.TryGetValue(cropCode, out crop);
            int currentIndex = growthDays[position];

            if (currentIndex <= crop.GrowTime)
            {
                currentIndex++;
                growthDays[position] = currentIndex;
                UpdateCropTile(position, cropCode, currentIndex);
            }
        }

        //wetTile 상태 업데이트
        List<Vector3Int> wetPositions = new List<Vector3Int>(wetTileDays.Keys);
        foreach (Vector3Int position in wetPositions)
        {
            interactableMap.SetTile(position, plowedTile);
            wetTileDays[position]++;
        }

        //이틀 동안 wetTile이 아닌 작물 타일 제거
        List<Vector3Int> dryPositions = new List<Vector3Int>(plantedCrops.Keys);
        foreach (Vector3Int position in dryPositions)
        {
            if (!wetTileDays.ContainsKey(position) || wetTileDays[position] > 1)
            {
                plowedMap.SetTile(position, null);
                plantedCrops.Remove(position);
                wetTileDays.Remove(position);
                growthDays.Remove(position);
            }
        }
    }

    public bool CompareTile(Vector3 position, bool isPlow)
    {
        Vector3Int gridPos = interactableMap.WorldToCell(position);
        TileBase currentTile = interactableMap.GetTile(gridPos);
        Tile targetTile = isPlow ? plowedTile : wetTile;
        if (currentTile == targetTile)
        {
            return true;
        }
        return false;
    }

    public bool IsCropPlanted(Vector3 position)
    {
        Vector3Int gridPos = interactableMap.WorldToCell(position);
        return plantedCrops.ContainsKey(gridPos);
    }

    public void FullGrow(Vector3 position)
    {
        Vector3Int gridPos = interactableMap.WorldToCell(position);
        cropTiles.TryGetValue("FARM00006", out TileRow row);
        growthDays[gridPos] = 4;
        plowedMap.SetTile(gridPos, row.tiles[0]);
    }
    #endregion

}