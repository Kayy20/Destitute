using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public unsafe struct Triangle
{
    
    public fixed int p[3];

    public fixed int adjacent[3];

    private const int NO_ADJ_TRI = -1;

    public Triangle(int point0, int point1, int point2)
    {
        p[0] = point0;
        p[1] = point1;
        p[2] = point2;

        adjacent[0] = NO_ADJ_TRI;
        adjacent[1] = NO_ADJ_TRI;
        adjacent[2] = NO_ADJ_TRI;
    }

    public Triangle(int point0, int point1, int point2, int adjacent0, int adjacent1, int adjacent2)
    {
        p[0] = point0;
        p[1] = point1;
        p[2] = point2;

        adjacent[0] = adjacent0;
        adjacent[1] = adjacent1;
        adjacent[2] = adjacent2;
        adjacent[2] = adjacent2;
    }

#if UNITY_EDITOR
    public List<int> DebugP
    {
        get
        {
            List<int> debugArray = new List<int>(3);
            for (int i = 0; i < 3; i++)
            {
                debugArray.Add(p[i]);
            }
            return debugArray;
        }
    }

    public List<int> DebugAdj
    {
        get
        {
            List<int> debugArray = new List<int>(3);
            for (int i = 0; i < 3; i++)
            {
                debugArray.Add(adjacent[i]);
            }
            return debugArray;
        }
    }
#endif
}
