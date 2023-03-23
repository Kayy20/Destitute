using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{

    public Node node;
    public int x;
    public int y;

    public int gCost = 0;
    public int hCost = 0;
    public int fCost = 0;

    public PathNode cameFromNode;

    public PathNode(Node node)
    {
        this.node = node;
    }
    public PathNode(Node node, int x, int y)
    {
        this.node = node;
        this.x = x;
        this.y = y;
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }


    public override string ToString()
    {
        return  x+ ", " + y;
    }

}
