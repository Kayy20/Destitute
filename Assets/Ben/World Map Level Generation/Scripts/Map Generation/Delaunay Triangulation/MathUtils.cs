using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MathUtils
{
    
    // Calculates the determinan of a 3x3 matrix
    public static float CalculateMatrix3x3Determinatnt(float m00, float m10, float m20,
        float m01, float m11, float m21,
        float m02, float m12, float m22)
    {
        return m00 * m11 * m22 + m10 * m21 * m02 + m20 * m01 * m12 - m20 * m11 * m02 - m10 * m01 * m22 - m00 * m21 * m12;
    }
    // Checks if the point is to the right of the edge
    public static bool IsPointToTheRightOfEdge(Vector2 edgePointA, Vector2 edgePointB, Vector2 point)
    {
        Vector2 aToB = (edgePointB - edgePointA).normalized;
        Vector2 aToP = (point - edgePointA).normalized;
        Vector3 ab_x_p = Vector3.Cross(aToB, aToP);
        return ab_x_p.z < -0.0001f;
    }
    // Checks if the point is contained inside the triangle
    public static bool IsPointInsideTriangle(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 point)
    {
        Vector3 ab_x_p = Vector3.Cross(p1 - p0, point);
        Vector3 bc_x_p = Vector3.Cross(p2 - p1, point);
        Vector3 ca_x_p = Vector3.Cross(p0 - p2, point);
        return ab_x_p.z == bc_x_p.z && ab_x_p.z == ca_x_p.z;
    }
    // Checks if the point is in a circumcircle containing each of the triangles points
    public static bool IsPointInsideCircumcircle(Vector2 p0, Vector2 p1, Vector2 p2, Vector2 point)
    {
        float a = p0.x - point.x;
        float d = p1.x - point.x;
        float g = p2.x - point.x;

        float b = p0.y - point.y;
        float e = p1.y - point.y;
        float h = p2.y - point.y;

        float c = a * a + b * b;
        float f = d * d + e * e;
        float i = g * g + h * h;

        float determinant = (a * e * i) + (b * f * g) + (c * d * h) - (g * e * c) - (h * f * a) - (i * d * b);
        return determinant >= 0;
    }
    // Calculates whether 2 line segments intersect and the point of intersection
    public static bool IntersectionBetweenLines(Vector2 pointA1, Vector2 pointB1, Vector2 pointA2, Vector2 pointB2, out Vector2 intersectionPoint)
    {
        intersectionPoint = new Vector2(float.MaxValue, float.MaxValue);

        bool vert1 = pointA1.x == pointB1.x;
        bool vert2 = pointA1.x == pointB1.x;

        float x = float.MaxValue;
        float y = float.MaxValue;

        if (vert1 && !vert2)
        {
            float m2 = (pointB2.y - pointA2.y) / (pointB2.x - pointA2.x);

            float A2 = m2;
            float C2 = pointA2.x * m2 - pointA2.y;

            x = pointA1.x;
            y = m2 * pointA1.x - C2;
        }
        else if (vert2 && !vert1)
        {
            float m1 = (pointB1.y - pointA1.y) / (pointB1.x - pointA1.x);

            float A1 = m1;
            float C1 = pointA1.x * m1 - pointA1.y;

            x = pointA2.x;
            y = m1 * pointA2.x - C1;
        }
        else if (!vert1 && !vert2)
        {
            float m1 = (pointB1.y - pointA1.y) / (pointB1.x - pointA1.x);

            float A1 = m1;
            float B1 = -1.0f;
            float C1 = pointA1.x * m1 - pointA1.y;

            float m2 = (pointB2.y - pointA2.y) / (pointB2.x - pointA2.x);

            float A2 = m2;
            float B2 = -1.0f;
            float C2 = pointA2.x * m2 - pointA2.y;

            float determinant = A1 * B2 - A2 * B1;

            if (determinant == 0)
            {
                // Lines do not intersect
                return false;
            }

            x = (B2 * C1 - B1 * C2) / determinant;
            y = (A1 * C2 - A2 * C1) / determinant;
        }
        // else not intersection
        bool result = false;

        if (x <= Mathf.Max(pointA1.x, pointB1.x) && x >= Mathf.Min(pointA1.x, pointB1.x) &&
            y <= Mathf.Max(pointA1.y, pointB1.y) && y >= Mathf.Min(pointA1.y, pointB1.y) &&
            x <= Mathf.Max(pointA2.x, pointB2.x) && x >= Mathf.Min(pointA2.x, pointB2.x) &&
            y <= Mathf.Max(pointA2.y, pointB2.y) && y >= Mathf.Min(pointA2.y, pointB2.y))
        {
            intersectionPoint.x = x;
            intersectionPoint.y = y;
            result = true;
        }

        return result;

    }
    // Checks if verticies are in ClockWise
    public static bool IsTriangleVerticiesCW(Vector2 p0, Vector2 p1, Vector2 p2)
    {
        return CalculateMatrix3x3Determinatnt(p0.x, p0.y, 1.0f,
            p1.x, p1.y, 1.0f,
            p2.x, p2.y, 1.0f) < 0.0f;
    }
    // Checks if Quadrilateral
    public static bool IsQuadrilateralConvex(Vector2 a, Vector2 b, Vector2 c, Vector2 d)
    {
        bool isConvex = false;

        bool abc = IsTriangleVerticiesCW(a, b, c);
        bool abd = IsTriangleVerticiesCW(a, b, d);
        bool bcd = IsTriangleVerticiesCW(b, c, d);
        bool cad = IsTriangleVerticiesCW(c, a, d);

        if (abc && abd && bcd && !cad)
        {
            isConvex = true;
        }
        else if (abc && abd && !bcd && !cad)
        {
            isConvex = true;
        }
        else if (abc && !abd && !bcd && !cad)
        {
            isConvex = true;
        }
        else if (!abc && !abd && !bcd && cad)
        {
            isConvex = true;
        }
        else if (!abc && !abd && bcd && !cad)
        {
            isConvex = true;
        }
        else if (!abc && abd && !bcd && !cad)
        {
            isConvex = true;
        }

        return isConvex;

    }
    // Calculates the area of a triangle
    public static float CalculateTriangleArea(Vector2 p0, Vector2 p1, Vector2 p2)
    {
        return Vector3.Cross(p1 - p0, p2 - p0).magnitude * 0.5f;
    }

}
