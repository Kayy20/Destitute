using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarPath
{

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    private Node startNode;
    private Node endNode;
    private List<Node> nodeList;

    private List<Node> openList;
    private List<Node> closedList;

    public AStarPath(Node startNode, Node endNode, List<Node> nodeList)
    {
        this.startNode = startNode;
        this.endNode = endNode;
        this.nodeList = nodeList;

        this.nodeList.Add(endNode);

    }

    public List<Node> FindPath()
    {
        openList = new List<Node> { startNode };
        closedList = new List<Node>();

        foreach (Node node in nodeList)
        {
            //CD.Log(CD.Programmers.BEN, "NODE!!! " + node.Location);
            node.Information.gCost = int.MaxValue;
            node.CalculateFCost();
            node.cameFromNode = null;
        }


        startNode.Information.gCost = 0;
        startNode.Information.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        //CD.Log(CD.Programmers.BEN, "" +openList.Count);
        //CD.Log(CD.Programmers.BEN, "" +openList.Count);

        while (openList.Count > 0)
        {
            Node currentNode = GetLowestFCostNode(openList);
            //CD.Log(CD.Programmers.BEN, currentNode.gameObject.name);
            if (currentNode == endNode)
            {
                // Reached Final Node
                CD.Log(CD.Programmers.BEN, "Route Generated!");
                return CalculatePath(endNode);
            }
            //CD.Log(CD.Programmers.BEN, "REMOVED FROM OPEN LIST");
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (Node neighbourNode in GetNeighbourList(currentNode))
            {

                int tentativeGCost = currentNode.Information.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.Information.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.Information.gCost = tentativeGCost;
                    neighbourNode.Information.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        //CD.Log(CD.Programmers.BEN, "Returned Null");
        return null;

    }

    private List<Node> GetNeighbourList(Node currentNode)
    {
        List<Node> neighbourList = new List<Node>();

        List<Node> neighbourNodes = currentNode.GetConnectedNodes();
        foreach (Node n in neighbourNodes)
        {
            if (n != null)
                if (!closedList.Contains(n))
                {
                    neighbourList.Add(n);
                }
        }

        return neighbourList;

    }

    private List<Node> CalculatePath(Node endNode)
    {
        List<Node> path = new List<Node>();
        path.Add(endNode);
        Node currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }


        //CD.Log(CD.Programmers.BEN, "Path Length: " + path.Count);

        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(Node a, Node b)
    {
        //CD.Log(CD.Programmers.BEN, a + " " + b);
        int xDistance = Mathf.Abs(a.Information.x - b.Information.x);
        int yDistance = Mathf.Abs(a.Information.y - b.Information.y);
        int remaining = Mathf.Abs(yDistance - xDistance);
        //CD.Log(CD.Programmers.BEN, xDistance + ", " + yDistance + ", " + remaining);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }


    private Node GetLowestFCostNode(List<Node> NodeList)
    {
        Node lowestFCostNode = NodeList[0];
        for (int i = 0; i < NodeList.Count; i++)
        {
            if (NodeList[i].Information.fCost < lowestFCostNode.Information.fCost)
            {
                lowestFCostNode = NodeList[i];
            }
        }
        return lowestFCostNode;
    }

}
