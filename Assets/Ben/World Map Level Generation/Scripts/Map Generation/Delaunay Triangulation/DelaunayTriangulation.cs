using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe class DelaunayTriangulation
{
    
    public TriangleSet TriangleSet
    {
        get { return m_triangleSet; }
    }

    public List<int> DiscardedTriangles
    {
        get { return m_trianglesToRemove; }
    }


    protected PointGrid m_grid;
    protected TriangleSet m_triangleSet;
    protected Stack<int> m_adjacentTriangleStack;
    protected const int NOT_FOUND = -1;
    protected const int NO_ADJACENT_TRIANGLE = -1;
    protected List<int> m_trianglesToRemove = new List<int>();
    protected Bounds m_mainPointCloudBounds = new Bounds();


    // Generates the triangulation of a point cloud that fulfills the Delaunay constraint.
    public void Triangulate(List<Vector2> inputPoints, float maximumAreaTesslation = 0.0f, List<List<Vector2>> constrainedEdges = null)
    {
        // Initialize containers
        if (m_triangleSet == null)
        {
            m_triangleSet = new TriangleSet(inputPoints.Count - 2);
        }
        else
        {
            m_triangleSet.Clear();
            m_triangleSet.SetCapacity(inputPoints.Count - 2);
        }

        if (m_adjacentTriangleStack == null)
        {
            m_adjacentTriangleStack = new Stack<int>(inputPoints.Count - 2);
        }
        else
        {
            m_adjacentTriangleStack.Clear();
        }

        if (m_trianglesToRemove == null)
        {
            m_trianglesToRemove = new List<int>();
        }
        else
        {
            m_trianglesToRemove.Clear();
        }

        // 1. Normalization
        m_mainPointCloudBounds = CalculateBoundsWithLeftBottomCornerAtOrigin(inputPoints);

        List<Vector2> normalizedPoints = new List<Vector2>(inputPoints);
        NormalizePoints(normalizedPoints, m_mainPointCloudBounds);

        // 2. Addition of points ot the space partitioning grid
        Bounds normalizedCloudBounds = CalculateBoundsWithLeftBottomCornerAtOrigin(normalizedPoints);
        m_grid = new PointGrid(Mathf.CeilToInt(Mathf.Sqrt(Mathf.Sqrt(inputPoints.Count))), normalizedCloudBounds.size);

        for (int i = 0; i < normalizedPoints.Count; i++)
        {
            m_grid.AddPoint(normalizedPoints[i]);
        }


        // 3. Supertriangle initialization
        Triangle2D superTriangle = new Triangle2D(new Vector2(-100.0f, -100.0f), new Vector2(100.0f, -100.0f), new Vector2(0.0f, 100.0f)); // CCW

        m_triangleSet.AddTriangle(superTriangle.p0, superTriangle.p1, superTriangle.p2, NO_ADJACENT_TRIANGLE, NO_ADJACENT_TRIANGLE, NO_ADJACENT_TRIANGLE);

        // 4. Adding points to the triangle set and triangulation
        // Points are added one at a time, and points that are close together are inserted together because they are sorted in the grid
        for (int i = 0; i < m_grid.Cells.Length; i++)
        {
            // If the cell contains a bin with points
            if (m_grid.Cells[i] != null)
            {
                // All the points in the bin are added together, one by one
                for (int j = 0; j < m_grid.Cells[i].Count; j++)
                {
                    AddPointToTriangulation(m_grid.Cells[i][j]);
                }
            }
        }


        if (maximumAreaTesslation > 0.0f)
        {
            Tesselate(maximumAreaTesslation);
        }

        // 5. Holes Creation (constrained edges)
        if (constrainedEdges != null)
        {
            List<List<int>> constrainedEdgeIndices = new List<List<int>>();

            // Adds the points of all thje polugons to the triangulation
            for (int i = 0; i < constrainedEdges.Count; ++i)
            {
                // 5.1: Normalize
                List<Vector2> normalizedConstrainedEdges = new List<Vector2>(constrainedEdges[i]);
                NormalizePoints(normalizedConstrainedEdges, m_mainPointCloudBounds);

                List<int> polygonEdgeIndices = new List<int>(normalizedConstrainedEdges.Count);

                // 5.2: Add the points to the Triangle set
                for (int j = 0; j < normalizedConstrainedEdges.Count - 0; ++j)
                {
                    if (normalizedConstrainedEdges[j] == normalizedConstrainedEdges[(j + 1) % normalizedConstrainedEdges.Count])
                    {
                        Debug.LogWarning($"The list of constrained edges contains a zero-length edge (2 consecutive coinciding points, indices {j} and {(j + 1) % normalizedConstrainedEdges.Count}). It will be ignored.");
                        continue;
                    }

                    int addedPointIndex = AddPointToTriangulation(normalizedConstrainedEdges[j]);
                    polygonEdgeIndices.Add(addedPointIndex);
                }

                constrainedEdgeIndices.Add(polygonEdgeIndices);
            }

            // 5.3: Create the constrained edges
            for (int i = 0; i < constrainedEdgeIndices.Count; ++i)
            {
                for (int j = 0; j < constrainedEdgeIndices[i].Count - 0; ++j)
                {
                    AddConstrainedEdgeToTriangulation(constrainedEdgeIndices[i][j], constrainedEdgeIndices[i][(j + 1) % constrainedEdgeIndices[i].Count]);
                }
            }

            // 5.4: Identify all the triangles in the polygon
            for (int i = 0; i < constrainedEdgeIndices.Count; ++i)
            {
                m_triangleSet.GetTrianglesInPolygon(constrainedEdgeIndices[i], m_trianglesToRemove);
            }
        }


        // 6. Remove SuperTriangle
        GetSupertriangleTrianges(m_trianglesToRemove);

        m_trianglesToRemove.Sort();

        // 7. Denormalization
        DenormalizePoints(m_triangleSet.Points, m_mainPointCloudBounds);
    }


    // Discards all the triangles that are inside a hole or belong to the super triangle
    public void GetTrianglesDiscardingHoles(List<Triangle2D> outputTriangles)
    {
        if (outputTriangles.Capacity < m_triangleSet.TriangleCount)
        {
            outputTriangles.Capacity = m_triangleSet.TriangleCount;
        }

        // 8. Output filtering
        for (int i = 0; i <m_triangleSet.TriangleCount; i++)
        {
            bool isTriangleToBeRemoved = false;

            // Is the triangle in the "To Remove" list?
            for (int j = 0; j < m_trianglesToRemove.Count; j++)
            {
                if (m_trianglesToRemove[j] >= i)
                {
                    isTriangleToBeRemoved = m_trianglesToRemove[j] == i;
                    break;
                }
            }

            if (!isTriangleToBeRemoved)
            {
                Triangle triangle = m_triangleSet.GetTriangle(i);
                outputTriangles.Add(new Triangle2D(m_triangleSet.Points[triangle.p[0]], m_triangleSet.Points[triangle.p[1]], m_triangleSet.Points[triangle.p[2]]));
            }
        }
    }

    // Reads all the triangles generated by the Triangulate method
    public void GetAllTriangles(List<Triangle2D> outputTriangles)
    {
        if (outputTriangles.Capacity < m_triangleSet.TriangleCount)
        {
            outputTriangles.Capacity = m_triangleSet.TriangleCount;
        }

        for (int i = 0; i < m_triangleSet.TriangleCount; ++i)
        {
            Triangle triangle = m_triangleSet.GetTriangle(i);
            outputTriangles.Add(new Triangle2D(m_triangleSet.Points[triangle.p[0]], m_triangleSet.Points[triangle.p[1]], m_triangleSet.Points[triangle.p[2]]));
        }
    }

    // Adds a point to the triangulation
    private int AddPointToTriangulation(Vector2 pointToInsert)
    {
        // 4.1 Check point existence
        int existingPointIndex = m_triangleSet.GetIndexOfPoint(pointToInsert);

        if (existingPointIndex != NOT_FOUND)
        {
            return existingPointIndex;
        }

        // 4.2: Search containing triangle
        int containingTriangleIndex = m_triangleSet.FindTriangleThatContainsPoint(pointToInsert, m_triangleSet.TriangleCount - 1); // Start at the last added triangle

        Triangle containingTriangle = m_triangleSet.GetTriangle(containingTriangleIndex);

        // 4.3: Store the point
        // Inserting a new point into a triangle splits it into 3 pieces, 3 new triangles
        int insertedPoint = m_triangleSet.AddPoint(pointToInsert);

        // 4.4: Create 2 triangles
        Triangle newTriangle1 = new Triangle(insertedPoint, containingTriangle.p[0], containingTriangle.p[1]);
        newTriangle1.adjacent[0] = NO_ADJACENT_TRIANGLE;
        newTriangle1.adjacent[1] = containingTriangle.adjacent[0];
        newTriangle1.adjacent[2] = containingTriangleIndex;
        int triangle1Index = m_triangleSet.AddTriangle(newTriangle1);

        Triangle newTriangle2 = new Triangle(insertedPoint, containingTriangle.p[2], containingTriangle.p[0]);
        newTriangle2.adjacent[0] = containingTriangleIndex;
        newTriangle2.adjacent[1] = containingTriangle.adjacent[2];
        newTriangle2.adjacent[2] = NO_ADJACENT_TRIANGLE;
        int triangle2Index = m_triangleSet.AddTriangle(newTriangle2);

        // Sets adjacency between the 2 new triangles
        newTriangle1.adjacent[0] = triangle2Index;
        newTriangle2.adjacent[2] = triangle1Index;
        m_triangleSet.SetTriangleAdjacency(triangle1Index, newTriangle1.adjacent);
        m_triangleSet.SetTriangleAdjacency(triangle2Index, newTriangle2.adjacent);

        // Sets the adjacency of the triangles that were adjacent to the original containing triangle
        if (newTriangle1.adjacent[1] != NO_ADJACENT_TRIANGLE)
        {
            m_triangleSet.ReplaceAdjacent(newTriangle1.adjacent[1], containingTriangleIndex, triangle1Index);
        }

        if (newTriangle2.adjacent[1] != NO_ADJACENT_TRIANGLE)
        {
            m_triangleSet.ReplaceAdjacent(newTriangle2.adjacent[1], containingTriangleIndex, triangle2Index);
        }

        // 4.5: Transform containing triangle into the third
        // Original triangle is transformed into the third triangle after the point has split the containing triangle into 3
        containingTriangle.p[0] = insertedPoint;
        containingTriangle.adjacent[0] = triangle1Index;
        containingTriangle.adjacent[2] = triangle2Index;
        m_triangleSet.ReplaceTriangle(containingTriangleIndex, containingTriangle);

        // 4.6: Add new triangles to a stack
        // Triangles that contain the inserted point are added to the stack for them to be processed by the Delaunay swapping algorithm
        if (containingTriangle.adjacent[1] != NO_ADJACENT_TRIANGLE) // If they do not have an opposite triangle in the outter edge, there is no need to check the Delaunay constraint for it
        {
            m_adjacentTriangleStack.Push(containingTriangleIndex);
        }

        if (newTriangle1.adjacent[1] != NO_ADJACENT_TRIANGLE)
        {
            m_adjacentTriangleStack.Push(triangle1Index);
        }

        if (newTriangle2.adjacent[1] != NO_ADJACENT_TRIANGLE)
        {
            m_adjacentTriangleStack.Push(triangle2Index);
        }

        // 4.7: Check Delaunay constraint
        FulfillDelaunayConstraint(m_adjacentTriangleStack);

        return insertedPoint;
    }

    // Processes a stack of triangles checking whether they fulfill the Delaunay constraint with respect to their adjacents
    private void FulfillDelaunayConstraint(Stack<int> adjacentTrianglesToProcess)
    {
        while (adjacentTrianglesToProcess.Count > 0)
        {
            int currentTriangleToSwap = adjacentTrianglesToProcess.Pop();
            Triangle triangle = m_triangleSet.GetTriangle(currentTriangleToSwap);

            const int OPPOSITE_TRIANGLE_INDEX = 1;

            if (triangle.adjacent[OPPOSITE_TRIANGLE_INDEX] == NO_ADJACENT_TRIANGLE)
            {
                continue;
            }

            const int NOT_IN_EDGE_VERTEX_INDEX = 0;
            Vector2 triangleVertexNotInEdge = m_triangleSet.GetPointByIndex(triangle.p[NOT_IN_EDGE_VERTEX_INDEX]);

            Triangle oppositeTriangle = m_triangleSet.GetTriangle(triangle.adjacent[OPPOSITE_TRIANGLE_INDEX]);
            Triangle2D oppositeTrianglePoints = m_triangleSet.GetTrianglePoints(triangle.adjacent[OPPOSITE_TRIANGLE_INDEX]);
            //CD.Log(CD.Programmers.BEN, "Triangle Points: " + oppositeTrianglePoints.p0 + ", " + oppositeTrianglePoints.p1 + ", " + oppositeTrianglePoints.p2);
            if (MathUtils.IsPointInsideCircumcircle(oppositeTrianglePoints.p0, oppositeTrianglePoints.p1, oppositeTrianglePoints.p2, triangleVertexNotInEdge))
            {
                // Finds the edge of the opposite triangle that is shared with the other triangle, this edge will be swapped
                int sharedEdgeVertexLocalIndex = 0;

                for (; sharedEdgeVertexLocalIndex < 3; ++sharedEdgeVertexLocalIndex)
                {
                    if (oppositeTriangle.adjacent[sharedEdgeVertexLocalIndex] == currentTriangleToSwap)
                    {
                        break;
                    }
                }

                // Adds the 2 triangles that were adjacent to the opposite triangle, to be processed too
                if (oppositeTriangle.adjacent[(sharedEdgeVertexLocalIndex + 1) % 3] != NO_ADJACENT_TRIANGLE)
                {
                    adjacentTrianglesToProcess.Push(oppositeTriangle.adjacent[(sharedEdgeVertexLocalIndex + 1) % 3]);
                }

                if (oppositeTriangle.adjacent[(sharedEdgeVertexLocalIndex + 2) % 3] != NO_ADJACENT_TRIANGLE)
                {
                    adjacentTrianglesToProcess.Push(oppositeTriangle.adjacent[(sharedEdgeVertexLocalIndex + 2) % 3]);
                }

                // 4.8: Swap edges
                SwapEdges(currentTriangleToSwap, triangle, NOT_IN_EDGE_VERTEX_INDEX, oppositeTriangle, sharedEdgeVertexLocalIndex);
            }
        }
    }

    // Replaces the shared edge witha new edge that joins both opposide verticies
    private void SwapEdges(int mainTriangleIndex, Triangle mainTriangle, int notInEdgeVertexLocalIndex, Triangle oppositeTriangle, int oppositeTriangleSharedEdgeVertexLocalIndex)
    {
        int oppositeVertex = (oppositeTriangleSharedEdgeVertexLocalIndex + 2) % 3;

        // Only one vertex of each triangle is moved
        int oppositeTriangleIndex = mainTriangle.adjacent[(notInEdgeVertexLocalIndex + 1) % 3];
        mainTriangle.p[(notInEdgeVertexLocalIndex + 1) % 3] = oppositeTriangle.p[oppositeVertex];
        oppositeTriangle.p[oppositeTriangleSharedEdgeVertexLocalIndex] = mainTriangle.p[notInEdgeVertexLocalIndex];
        oppositeTriangle.adjacent[oppositeTriangleSharedEdgeVertexLocalIndex] = mainTriangle.adjacent[notInEdgeVertexLocalIndex];
        mainTriangle.adjacent[notInEdgeVertexLocalIndex] = oppositeTriangleIndex;
        mainTriangle.adjacent[(notInEdgeVertexLocalIndex + 1) % 3] = oppositeTriangle.adjacent[oppositeVertex];
        oppositeTriangle.adjacent[oppositeVertex] = mainTriangleIndex;

        m_triangleSet.ReplaceTriangle(mainTriangleIndex, mainTriangle);
        m_triangleSet.ReplaceTriangle(oppositeTriangleIndex, oppositeTriangle);

        // Adjacent triangles are updated too
        if (mainTriangle.adjacent[(notInEdgeVertexLocalIndex + 1) % 3] != NO_ADJACENT_TRIANGLE)
        {
            m_triangleSet.ReplaceAdjacent(mainTriangle.adjacent[(notInEdgeVertexLocalIndex + 1) % 3], oppositeTriangleIndex, mainTriangleIndex);
        }

        if (oppositeTriangle.adjacent[oppositeTriangleSharedEdgeVertexLocalIndex] != NO_ADJACENT_TRIANGLE)
        {
            m_triangleSet.ReplaceAdjacent(oppositeTriangle.adjacent[oppositeTriangleSharedEdgeVertexLocalIndex], mainTriangleIndex, oppositeTriangleIndex);
        }
    }

    // Adds and edge to the triangulation
    private void AddConstrainedEdgeToTriangulation(int endpointAIndex, int endpointBIndex)
    {
        // Detects if the edge already exists
        if (m_triangleSet.FindTriangleThatContainsEdge(endpointAIndex, endpointBIndex).TriangleIndex != NOT_FOUND)
        {
            return;
        }

        Vector2 edgeEndpointA = m_triangleSet.GetPointByIndex(endpointAIndex);
        Vector2 edgeEndpointB = m_triangleSet.GetPointByIndex(endpointBIndex);

        // 5.3.1: Search for the triangle that contains the beginning of the new edge
        int triangleContainingA = m_triangleSet.FindTriangleThatContainsLineEndpoint(endpointAIndex, endpointBIndex);


        // 5.3.2: Get all the triangle edges intersected by the constrained edge
        List<TriangleEdge> intersectedTriangleEdges = new List<TriangleEdge>();
        m_triangleSet.GetIntersectingEdges(edgeEndpointA, edgeEndpointB, triangleContainingA, intersectedTriangleEdges);

        List<TriangleEdge> newEdges = new List<TriangleEdge>();

        while (intersectedTriangleEdges.Count > 0)
        {
            TriangleEdge currentIntersectedTriangleEdge = intersectedTriangleEdges[intersectedTriangleEdges.Count - 1];
            intersectedTriangleEdges.RemoveAt(intersectedTriangleEdges.Count - 1);

            // 5.3.3: Form quadrilaterals and swap intersected edges
            // Deduces the data for both triangles
            currentIntersectedTriangleEdge = m_triangleSet.FindTriangleThatContainsEdge(currentIntersectedTriangleEdge.EdgeVertexA, currentIntersectedTriangleEdge.EdgeVertexB);
            Triangle intersectedTriangle = m_triangleSet.GetTriangle(currentIntersectedTriangleEdge.TriangleIndex);
            Triangle oppositeTriangle = m_triangleSet.GetTriangle(intersectedTriangle.adjacent[currentIntersectedTriangleEdge.EdgeIndex]);
            Triangle2D trianglePoints = m_triangleSet.GetTrianglePoints(currentIntersectedTriangleEdge.TriangleIndex);

            // Gets the opposite vertex of adjacent triangle, knowing the fisrt vertex of the shared edge
            int oppositeVertex = NOT_FOUND;

            int oppositeSharedEdgeVertex = NOT_FOUND; // The first vertex in the shared edge of the opposite triangle

            for (int j = 0; j < 3; ++j)
            {
                if (oppositeTriangle.p[j] == intersectedTriangle.p[(currentIntersectedTriangleEdge.EdgeIndex + 1) % 3]) // Comparing with the endpoint B of the edge, since the edge AB is BA in the adjacent triangle
                {
                    oppositeVertex = oppositeTriangle.p[(j + 2) % 3];
                    oppositeSharedEdgeVertex = j;
                    break;
                }
            }

            Vector2 oppositePoint = m_triangleSet.GetPointByIndex(oppositeVertex);

            if (MathUtils.IsQuadrilateralConvex(trianglePoints.p0, trianglePoints.p1, trianglePoints.p2, oppositePoint))
            {
                // Swap
                int notInEdgeTriangleVertex = (currentIntersectedTriangleEdge.EdgeIndex + 2) % 3;
                SwapEdges(currentIntersectedTriangleEdge.TriangleIndex, intersectedTriangle, notInEdgeTriangleVertex, oppositeTriangle, oppositeSharedEdgeVertex);

                // Refreshes triangle data after swapping
                intersectedTriangle = m_triangleSet.GetTriangle(currentIntersectedTriangleEdge.TriangleIndex);

                // Check new diagonal against the intersecting edge
                Vector2 intersectionPoint;
                int newTriangleSharedEdgeVertex = (currentIntersectedTriangleEdge.EdgeIndex + 2) % 3; // Read SwapEdges method to understand the +2
                Vector2 newTriangleSharedEdgePointA = m_triangleSet.GetPointByIndex(intersectedTriangle.p[newTriangleSharedEdgeVertex]);
                Vector2 newTriangleSharedEdgePointB = m_triangleSet.GetPointByIndex(intersectedTriangle.p[(newTriangleSharedEdgeVertex + 1) % 3]);

                TriangleEdge newEdge = new TriangleEdge(NOT_FOUND, NOT_FOUND, intersectedTriangle.p[newTriangleSharedEdgeVertex], intersectedTriangle.p[(newTriangleSharedEdgeVertex + 1) % 3]);

                if (newTriangleSharedEdgePointA != edgeEndpointB && newTriangleSharedEdgePointB != edgeEndpointB && // Watch out! It thinks the line intersects with the edge when an endpoint coincides with a triangle vertex, this problem is avoided thanks to this conditions
                    newTriangleSharedEdgePointA != edgeEndpointA && newTriangleSharedEdgePointB != edgeEndpointA &&
                    MathUtils.IntersectionBetweenLines(edgeEndpointA, edgeEndpointB, newTriangleSharedEdgePointA, newTriangleSharedEdgePointB, out intersectionPoint))
                {
                    // New triangles edge still intersects with the constrained edge, so it is returned to the list
                    intersectedTriangleEdges.Insert(0, newEdge);
                }
                else
                {
                    newEdges.Add(newEdge);
                }
            }
            else
            {
                // Back to the list
                intersectedTriangleEdges.Insert(0, currentIntersectedTriangleEdge);
            }
        }

        // 5.3.4. Check Delaunay constraint and swap edges
        for (int i = 0; i < newEdges.Count; ++i)
        {
            // Checks if the constrained edge coincides with the new edge
            Vector2 triangleEdgePointA = m_triangleSet.GetPointByIndex(newEdges[i].EdgeVertexA);
            Vector2 triangleEdgePointB = m_triangleSet.GetPointByIndex(newEdges[i].EdgeVertexB);

            if ((triangleEdgePointA == edgeEndpointA && triangleEdgePointB == edgeEndpointB) ||
                (triangleEdgePointB == edgeEndpointA && triangleEdgePointA == edgeEndpointB))
            {
                continue;
            }

            // Deduces the data for both triangles
            TriangleEdge currentEdge = m_triangleSet.FindTriangleThatContainsEdge(newEdges[i].EdgeVertexA, newEdges[i].EdgeVertexB);
            Triangle currentEdgeTriangle = m_triangleSet.GetTriangle(currentEdge.TriangleIndex);
            int triangleVertexNotShared = (currentEdge.EdgeIndex + 2) % 3;
            Vector2 trianglePointNotShared = m_triangleSet.GetPointByIndex(currentEdgeTriangle.p[triangleVertexNotShared]);
            Triangle oppositeTriangle = m_triangleSet.GetTriangle(currentEdgeTriangle.adjacent[currentEdge.EdgeIndex]);
            Triangle2D oppositeTrianglePoints = m_triangleSet.GetTrianglePoints(currentEdgeTriangle.adjacent[currentEdge.EdgeIndex]);

            //List<int> debugP = currentEdgeTriangle.DebugP;
            //List<int> debugA = currentEdgeTriangle.DebugAdjacent;
            //List<int> debugP2 = oppositeTriangle.DebugP;
            //List<int> debugA2 = oppositeTriangle.DebugAdjacent;

            if (MathUtils.IsPointInsideCircumcircle(oppositeTrianglePoints.p0, oppositeTrianglePoints.p1, oppositeTrianglePoints.p2, trianglePointNotShared))
            {
                // Finds the edge of the opposite triangle that is shared with the other triangle, this edge will be swapped
                int sharedEdgeVertexLocalIndex = 0;

                for (; sharedEdgeVertexLocalIndex < 3; ++sharedEdgeVertexLocalIndex)
                {
                    if (oppositeTriangle.adjacent[sharedEdgeVertexLocalIndex] == currentEdge.TriangleIndex)
                    {
                        break;
                    }
                }

                // Swap
                SwapEdges(currentEdge.TriangleIndex, currentEdgeTriangle, triangleVertexNotShared, oppositeTriangle, sharedEdgeVertexLocalIndex);
            }
        }
    }

    // Gets all the triangles that contain any of the verticies of the supertriangle
    private void GetSupertriangleTrianges(List<int> outputTriangles)
    {
        for (int i = 0; i < 3; ++i) // Vertices of the supertriangle
        {
            List<int> trianglesThatShareVertex = new List<int>();
            m_triangleSet.GetTrianglesWithVertex(i, trianglesThatShareVertex);

            for (int j = 0; j < trianglesThatShareVertex.Count; ++j)
            {
                if (!outputTriangles.Contains(trianglesThatShareVertex[j]))
                {
                    outputTriangles.Add(trianglesThatShareVertex[j]);
                }
            }
        }
    }

    // Calculates the bounds of a point cloud
    private Bounds CalculateBoundsWithLeftBottomCornerAtOrigin(List<Vector2> points)
    {
        Vector2 newMin = new Vector2(float.MaxValue, float.MaxValue);
        Vector2 newMax = new Vector2(float.MinValue, float.MinValue);

        for (int i = 0; i < points.Count; ++i)
        {
            if (points[i].x > newMax.x)
            {
                newMax.x = points[i].x;
            }

            if (points[i].y > newMax.y)
            {
                newMax.y = points[i].y;
            }

            if (points[i].x < newMin.x)
            {
                newMin.x = points[i].x;
            }

            if (points[i].y < newMin.y)
            {
                newMin.y = points[i].y;
            }
        }

        Vector2 size = new Vector2(Mathf.Abs(newMax.x - newMin.x), Mathf.Abs(newMax.y - newMin.y));

        return new Bounds(size * 0.5f + newMin, size);
    }

    // Normalizes a list of points according to a bounding box
    private void NormalizePoints(List<Vector2> inputOutputPoints, Bounds bounds)
    {
        float maximunDimension = Mathf.Max(bounds.size.x, bounds.size.y);

        for (int i = 0; i < inputOutputPoints.Count; i++)
        {
            inputOutputPoints[i] = (inputOutputPoints[i] - (Vector2)bounds.min) / maximunDimension;
        }
    }

    // Denormalizes a list of points according to a bounding box
    private void DenormalizePoints(List<Vector2> inputOutputPoints, Bounds bounds)
    {
        float maximumDimension = Mathf.Max(bounds.size.x, bounds.size.y);

        for (int i = 0; i < inputOutputPoints.Count; ++i)
        {
            inputOutputPoints[i] = inputOutputPoints[i] * maximumDimension + (Vector2)bounds.min;
        }
    }

    // For each triangle, it splits its edges in 2 pieces, generating 4 subtriangles.
    protected void Tesselate(float maximumTriangleArea)
    {
        int i = 2; // Skips supertriangle

        while (i < m_triangleSet.TriangleCount - 1)
        {
            i++;

            // Skips all supertriangle triangles
            bool isSupertriangle = false;
            Triangle triangleData = m_triangleSet.GetTriangle(i);

            for (int j = 0; j < 3; j++)
            {
                if (triangleData.p[j] == 0 || triangleData.p[j] == 1 || triangleData.p[j] == 2) // 0, 1 and 2 are vertices of the supertriangle
                {
                    isSupertriangle = true;
                    break;
                }
            }

            if (isSupertriangle)
            {
                continue;
            }

            Triangle2D trianglePoints = m_triangleSet.GetTrianglePoints(i);
            float TriangleArea = MathUtils.CalculateTriangleArea(trianglePoints.p0, trianglePoints.p1, trianglePoints.p2);

            if (TriangleArea > maximumTriangleArea)
            {
                AddPointToTriangulation(trianglePoints.p0 + (trianglePoints.p1 - trianglePoints.p0) * 0.5f);
                AddPointToTriangulation(trianglePoints.p1 + (trianglePoints.p2 - trianglePoints.p1) * 0.5f);
                AddPointToTriangulation(trianglePoints.p2 + (trianglePoints.p0 - trianglePoints.p2) * 0.5f);

                i = 2; // The tesselation restarts
            }


        }
    }


}
