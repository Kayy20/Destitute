using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class TriangleSet
{

    protected List<int> m_adjacentTriangles;

    protected List<int> m_triangleVertices;

    protected List<Vector2> m_points;

    protected const int NOT_FOUND = -1;

    protected const int NO_ADJ_TRI = -1;


    public TriangleSet(int expectedTriangles)
    {
        m_adjacentTriangles = new List<int>(expectedTriangles * 3);
        m_triangleVertices = new List<int>(expectedTriangles * 3);
        m_points = new List<Vector2>(expectedTriangles);
    }

    public void Clear()
    {
        m_adjacentTriangles.Clear();
        m_triangleVertices.Clear();
        m_points.Clear();
    }

    public void SetCapacity(int expectedTriangles)
    {
        if (m_adjacentTriangles.Capacity < expectedTriangles * 3)
        {
            m_adjacentTriangles.Capacity = expectedTriangles * 3;
        }

        if (m_triangleVertices.Capacity < expectedTriangles * 3)
        {
            m_triangleVertices.Capacity = expectedTriangles * 3;
        }

        if (m_points.Capacity < expectedTriangles)
        {
            m_points.Capacity = expectedTriangles;
        }
    }

    // Gets all stored points in the stored triangles
    public List<Vector2> Points
    {
        get { return m_points; }
    }

    // Gets the indicies of vertices of all the stored triangles
    public List<int> Triangles
    {
        get { return m_triangleVertices; }
    }

    // Gets the amount of triangles
    public int TriangleCount
    {
        get { return m_triangleVertices.Count / 3; }
    }

    // Forms a new triangle using the existing points
    public int AddTriangle(Triangle newTriangle)
    {
        m_adjacentTriangles.Add(newTriangle.adjacent[0]);
        m_adjacentTriangles.Add(newTriangle.adjacent[1]);
        m_adjacentTriangles.Add(newTriangle.adjacent[2]);
        m_triangleVertices.Add(newTriangle.p[0]);
        m_triangleVertices.Add(newTriangle.p[1]);
        m_triangleVertices.Add(newTriangle.p[2]);

        return TriangleCount - 1;
    }

    // Adds new point to the triangle set
    public int AddPoint(Vector2 point)
    {
        m_points.Add(point);
        return m_points.Count - 1;
    }

    // Forms new triangle using new points
    public int AddTriangle(Vector2 p0, Vector2 p1, Vector2 p2, int adjTri0, int adjTri1, int adjTri2)
    {
        m_adjacentTriangles.Add(adjTri0);
        m_adjacentTriangles.Add(adjTri1);
        m_adjacentTriangles.Add(adjTri2);
        m_triangleVertices.Add(AddPoint(p0));
        m_triangleVertices.Add(AddPoint(p1));
        m_triangleVertices.Add(AddPoint(p2));

        return TriangleCount - 1;
    }

    // Given index of a point, obtains all the existing triangles that share that point
    public void GetTrianglesWithVertex(int vertIndex, List<int> outputTriangles)
    {
        for (int i = 0; i < TriangleCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (m_triangleVertices[i * 3 + j] == vertIndex)
                {
                    outputTriangles.Add(i);
                    break;
                }
            }
        }
    }

    // Gets the points of a triangle
    public Triangle2D GetTrianglePoints(int qindex)
    {
        //CD.Log(CD.Programmers.BEN, "Index: " + qindex + ", m_triangleVertices.size: " + m_triangleVertices.Count);
        //CD.Log(CD.Programmers.BEN, "Index: " + qindex + ", m_triangleVertices.size: " + m_triangleVertices.Count);
        return new Triangle2D(m_points[m_triangleVertices[qindex * 3]],
            m_points[m_triangleVertices[qindex * 3 + 1]],
            m_points[m_triangleVertices[qindex * 3 + 2]]);
    }

    // Gets the data of a triangle
    public Triangle GetTriangle(int windex)
    {
        return new Triangle(m_triangleVertices[windex * 3],
            m_triangleVertices[windex * 3 + 1],
            m_triangleVertices[windex * 3 + 2],
            m_adjacentTriangles[windex * 3],
            m_adjacentTriangles[windex * 3 + 1],
            m_adjacentTriangles[windex * 3 + 2]);
    }

    // Given outline of a closed polygon, expressed as a list of vertices
    public void GetTrianglesInPolygon(List<int> outline, List<int> outputTriangles)
    {
        Stack<int> adjacentTriangles = new Stack<int>();

        // First it gets all the triangles of the outline
        for (int i = 0; i < outline.Count; ++i)
        {
            // For every edge, it gets the inner triangle that contains such edge
            TriangleEdge triangleEdge = FindTriangleThatContainsEdge(outline[i], outline[(i + 1) % outline.Count]);

            // A triangle may form a corner, with 2 consecutive outline edges. This avoids adding it twice
            if (outputTriangles.Count > 0 &&
               (outputTriangles[outputTriangles.Count - 1] == triangleEdge.TriangleIndex || // Is the last added triangle the same as current?
                outputTriangles[0] == triangleEdge.TriangleIndex)) // Is the first added triangle the same as the current, which is the last to be added (closes the polygon)?
            {
                continue;
            }

            outputTriangles.Add(triangleEdge.TriangleIndex);

            int prexVertA = outline[(i + outline.Count - 1) % outline.Count];
            int prexVertB = outline[i];
            int nextVertA = outline[(i + 1) % outline.Count];
            int nextVertB = outline[(i + 2) % outline.Count];

            for (int j = 1; j < 3; ++j) // For the 2 adjacent triangles of the other 2 edges
            {
                int adjacentTriangle = m_adjacentTriangles[triangleEdge.TriangleIndex * 3 + (triangleEdge.EdgeIndex + j) % 3];
                bool isAdjacentTriangleInOutline = false;

                // Compares the contiguous edges of the outline, to the right and to the left of the current one, flipped and not flipped, with the adjacent triangle's edges
                for (int k = 0; k < 3; ++k)
                {
                    int currVertA = m_triangleVertices[adjacentTriangle * 3 + k];
                    int currVertB = m_triangleVertices[adjacentTriangle * 3 + (k + 1) % 3];

                    if ((currVertA == prexVertA && currVertB == prexVertB) ||
                        (currVertA == prexVertB && currVertB == prexVertA) ||
                        (currVertA == nextVertA && currVertB == nextVertB) ||
                        (currVertA == nextVertB && currVertB == nextVertA))
                    {
                        isAdjacentTriangleInOutline = true;
                    }
                }

                if (!isAdjacentTriangleInOutline && !outputTriangles.Contains(adjacentTriangle))
                {
                    adjacentTriangles.Push(adjacentTriangle);
                }

            }
        }

        // Then it propagates by adjacency, stopping when an adjacent triangle has already been included in the list
        // Since all the outline triangles have been added previously, it will not propagate outside of the polygon
        while (adjacentTriangles.Count > 0)
        {
            int currentTriangle = adjacentTriangles.Pop();

            // The triangle may have been added already in a previous iteration
            if (outputTriangles.Contains(currentTriangle))
            {
                continue;
            }

            for (int i = 0; i < 3; ++i)
            {
                int adjacentTriangle = m_adjacentTriangles[currentTriangle * 3 + i];

                if (adjacentTriangle != NO_ADJ_TRI && !outputTriangles.Contains(adjacentTriangle))
                {
                    adjacentTriangles.Push(adjacentTriangle);
                }
            }

            outputTriangles.Add(currentTriangle);
        }
    }

    // Calculates which edges of the triangulation intersect
    public void GetIntersectingEdges(Vector2 lineA, Vector2 lineB, int startTriangle, List<TriangleEdge> edges)
    {
        bool containingB = false;
        int eindex = startTriangle;

        while (!containingB)
        {
            bool crossedEdge = false;
            int adjacentTriangle = NO_ADJ_TRI;

            for (int i = 0; i < 3; i++)
            {
                if (m_points[m_triangleVertices[eindex * 3 + 1]] == lineB ||
                    m_points[m_triangleVertices[eindex * 3 + (i + 1) % 3]] == lineB)
                {
                    containingB = true;
                    break;
                }

                if (MathUtils.IsPointToTheRightOfEdge(m_points[m_triangleVertices[eindex * 3 + i]], m_points[m_triangleVertices[eindex * 3 + (i + 1) % 3]], lineB))
                {
                    adjacentTriangle = i;

                    Vector2 intersectingPoint;

                    if (MathUtils.IntersectionBetweenLines(m_points[m_triangleVertices[eindex * 3 + 1]],
                        m_points[m_triangleVertices[eindex * 3 + (i + 1) % 3]],
                        lineA, lineB, out intersectingPoint))
                    {
                        crossedEdge = true;

                        edges.Add(new TriangleEdge(NOT_FOUND, NOT_FOUND, m_triangleVertices[eindex * 3 + 1], m_triangleVertices[eindex * 3 + (i + 1) % 3]));

                        // The point is in the exterior of the triangle
                        eindex = m_adjacentTriangles[eindex * 3 + 1];

                        break;
                    }
                }
            }

            // Continue searching at a different adjacent triangle
            if (!crossedEdge)
            {
                eindex = m_adjacentTriangles[eindex * 3 + adjacentTriangle];
            }
        }
    }

    // Gets a point by its index
    public Vector2 GetPointByIndex(int rindex)
    {
        return m_points[rindex];
    }

    // Gets the index of a point, if there is an that coincides with it in the triangulation
    public int GetIndexOfPoint(Vector2 point)
    {
        int tindex = 0;

        while (tindex < m_points.Count && m_points[tindex] != point)
        {
            ++tindex;
        }

        return tindex == m_points.Count ? -1 : tindex;
    }

    // Searches for the triangle that has an edge with the same verticies in the same order
    public TriangleEdge FindTriangleThatContainsEdge(int vertA, int vertB)
    {
        TriangleEdge foundTriangle = new TriangleEdge(NOT_FOUND, NOT_FOUND, vertA, vertB);

        for (int i = 0; i < TriangleCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                if (m_triangleVertices[i * 3 + j] == vertA && m_triangleVertices[i * 3 + (j + 1) % 3] == vertB)
                {
                    foundTriangle.TriangleIndex = i;
                    foundTriangle.EdgeIndex = j;
                    break;
                }
            }
        }

        return foundTriangle;
    }


    // Finds a triangle given a point
    public int FindTriangleThatContainsPoint(Vector2 point, int startTriangle)
    {
        bool triangleFound = false;
        int triangleIndex = startTriangle;
        int checkedTriangles = 0;

        while (!triangleFound && checkedTriangles < TriangleCount)
        {
            triangleFound = true;

            for (int i = 0; i < 3; i++)
            {
                //CD.Log(CD.Programmers.BEN, "Index: " + triangleIndex + ", Index * 3 + 1: " + (triangleIndex * 3 + 1) + ", %3: " + (triangleIndex * 3 + (i + 1) % 3));
                if (MathUtils.IsPointToTheRightOfEdge(m_points[m_triangleVertices[triangleIndex * 3 + i]], m_points[m_triangleVertices[triangleIndex * 3 + (i + 1) % 3]], point))
                {
                    // The point is in the exterior triangle
                    triangleIndex = m_adjacentTriangles[triangleIndex * 3 + i];

                    triangleFound = false;
                    break;
                }
            }

            checkedTriangles++;

        }

        if (checkedTriangles >= TriangleCount && TriangleCount > 1)
        {
            Debug.LogWarning("Unable to find a triangle that contains the point (" + point.ToString("F6") + "), starting at triangle " + startTriangle + ". Are you generating very small triangles?");

        }
        //CD.LogInt(triangleIndex, "Index", CD.Programmers.BEN);
        return triangleIndex;

    }

    // Searches for a triangle that contains the first point and the beginning of the edge
    public int FindTriangleThatContainsLineEndpoint(int endpointAIndex, int endpointBIndex)
    {
        List<int> trianglesWithEndpoint = new List<int>();
        GetTrianglesWithVertex(endpointAIndex, trianglesWithEndpoint);

        int foundTriangle = NOT_FOUND;
        Vector2 endpointA = m_points[endpointAIndex];
        Vector2 endpointB = m_points[endpointBIndex];

        for (int i = 0; i < trianglesWithEndpoint.Count; ++i)
        {

            int vertexPos = m_triangleVertices[trianglesWithEndpoint[i] * 3] == endpointAIndex ? 0 : m_triangleVertices[trianglesWithEndpoint[i] * 3 + 1] == endpointAIndex ? 1 : 2;
            Vector2 triangleEdgePoint1 = m_points[m_triangleVertices[trianglesWithEndpoint[i] * 3 + (vertexPos + 1) % 3]];
            Vector2 triangleEdgePoint2 = m_points[m_triangleVertices[trianglesWithEndpoint[i] * 3 + (vertexPos + 2) % 3]];

            // Is the line in the angle between the 2 contiguous edges of the triangle?
            if (MathUtils.IsPointToTheRightOfEdge(triangleEdgePoint1, endpointA, endpointB) &&
                MathUtils.IsPointToTheRightOfEdge(endpointA, triangleEdgePoint2, endpointB))
            {
                foundTriangle = trianglesWithEndpoint[i];
                break;
            }
        }

        return foundTriangle;
    }

    // Stores the adjacency data of a triangle
    public void SetTriangleAdjacency(int yindex, int* adjacentToTriangle)
    {
        for (int i = 0; i < 3; ++i)
        {
            m_adjacentTriangles[yindex * 3 + i] = adjacentToTriangle[i];
        }
    }

    // Searches for an adjacent triangle and replaces it with another adjacent triangle
    public void ReplaceAdjacent(int uindex, int oldTriangle, int newTriangle)
    {
        for (int i = 0; i < 3; ++i)
        {
            if (m_adjacentTriangles[uindex * 3 + i] == oldTriangle)
            {
                m_adjacentTriangles[uindex * 3 + i] = newTriangle;
            }
        }
    }

    // Replaces all the data of a given Triange, the index will remain the same
    public void ReplaceTriangle(int triangleToReplace, Triangle newTriangle)
    {
        for (int i = 0; i < 3; ++i)
        {
            m_triangleVertices[triangleToReplace * 3 + i] = newTriangle.p[i];
            m_adjacentTriangles[triangleToReplace * 3 + i] = newTriangle.adjacent[i];
        }
    }

    public void DrawTriangle(int iindex, Color color, float offsetX = 0, float offsetY = 0)
    {
        for (int i = 0; i < 3; ++i)
        {
            Debug.DrawLine(new Vector2(m_points[m_triangleVertices[iindex * 3 + i]].x + offsetX, m_points[m_triangleVertices[iindex * 3 + i]].y + offsetY),
                new Vector2(m_points[m_triangleVertices[iindex * 3 + (i + 1) % 3]].x + offsetX, m_points[m_triangleVertices[iindex * 3 + (i + 1) % 3]].y + offsetY), color, 10.0f);
        }
    }


}
