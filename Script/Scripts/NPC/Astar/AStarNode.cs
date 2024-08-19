public class AStarNode
{
    public float xPos;
    public float yPos;

    public int xIndex;
    public int yIndex;

    public bool isWalkable;

    public AStarNode parent;

    // 이전부터 지금
    public float gCost;
    // 지금부터 다음 (휴리스틱 코스트)
    public float hCost;
    public float fCost { get => gCost + hCost; }


    public AStarNode()
    {
        xPos = -1;
        yPos = -1;
        xIndex = 0;
        yIndex = 0;
        hCost = 0;
        gCost = 0;
        parent = null;
    }

    public void Reset()
    {
        hCost = 0;
        gCost = int.MaxValue;
        parent = null;
    }
}
