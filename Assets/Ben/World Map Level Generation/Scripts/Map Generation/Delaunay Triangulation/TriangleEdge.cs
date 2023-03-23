using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TriangleEdge
{

    public int TriangleIndex;

    public int EdgeIndex;

    public int EdgeVertexA;

    public int EdgeVertexB;
    
    public TriangleEdge(int triangleIndex, int edgeIndex, int edgeVertexA, int edgeVertexB)
    {
        TriangleIndex = triangleIndex;
        EdgeIndex = edgeIndex;
        EdgeVertexA = edgeVertexA;
        EdgeVertexB = edgeVertexB;
    }

}
