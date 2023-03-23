using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Triangle2D
{
    // First Vector
    public Vector2 p0;

    // Second Vector
    public Vector2 p1;

    // Third Vector
    public Vector2 p2;


    public Triangle2D(Vector2 point0, Vector2 point1, Vector2 point2)
    {
        p0 = point0;
        p1 = point1;
        p2 = point2;
    }

    public Vector2 this[int index]
    {
        get
        {
            return index == 0 ? p0 : index == 1 ? p1 : p2;
        }
    }


}
