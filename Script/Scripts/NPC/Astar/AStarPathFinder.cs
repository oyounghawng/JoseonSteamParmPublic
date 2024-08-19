using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinder
{
    private float Heuristic(AStarNode a, AStarNode b, bool diagonal = false)
    {
        var dx = Mathf.Abs(a.xPos - b.xPos);
        var dy = Mathf.Abs(a.yPos - b.yPos);

        if (!diagonal) return 1 * (dx + dy);

        return Mathf.Max(Mathf.Abs(a.xPos - b.xPos), Mathf.Abs(a.yPos - b.yPos));
    }

    public List<AStarNode> CreatePath(AStarNode start, AStarNode end, bool diagonal = false)
    {
        if (start == null || end == null) return null;

        if (GridManager.Instance != null)
        {
            GridManager.Instance.ResetNode();

            List<AStarNode> open = new List<AStarNode>();
            List<AStarNode> close = new List<AStarNode>();


            AStarNode startNode = start;
            AStarNode endNode = end;

            startNode.gCost = 0;
            startNode.hCost = Heuristic(start, end);

            open.Add(startNode);

            while (open.Count > 0)
            {
                int shortest = 0;

                for (int i = 1; i < open.Count; i++)
                {
                    if (open[i].fCost < open[shortest].fCost)
                    {
                        shortest = i;
                    }
                }

                AStarNode currentNode = open[shortest];

                if (currentNode == endNode)
                {
                    List<AStarNode> path = new List<AStarNode>();
                    path.Add(endNode);

                    var tempNode = endNode;

                    while (tempNode.parent != null)
                    {
                        path.Add(tempNode.parent);
                        tempNode = tempNode.parent;
                    }

                    path.Reverse();

                    return path;
                }

                open.Remove(currentNode);
                close.Add(currentNode);

                var neighbors = GridManager.Instance.GetNeighborNodes(currentNode, diagonal);

                for (int i = 0; i < neighbors.Count; i++)
                {
                    if (close.Contains(neighbors[i]) || !neighbors[i].isWalkable) continue;
                    var gCost = currentNode.gCost + Heuristic(currentNode, neighbors[i], diagonal);

                    if (gCost < neighbors[i].gCost)
                    {
                        neighbors[i].parent = currentNode;
                        neighbors[i].gCost = gCost;
                        neighbors[i].hCost = Heuristic(neighbors[i], endNode, diagonal);

                        if (!open.Contains(neighbors[i]))
                            open.Add(neighbors[i]);
                    }
                }
            }
        }
        return null;
    }

    public List<AStarNode> CreatePath(Vector3Int start, Vector3Int end, bool diagonal)
    {
        AStarNode starNode = GridManager.Instance.GetNodeFromWorld(start);
        AStarNode endNode = GridManager.Instance.GetNodeFromWorld(end);

        var path = CreatePath(start, end, diagonal);

        return path;
    }
}