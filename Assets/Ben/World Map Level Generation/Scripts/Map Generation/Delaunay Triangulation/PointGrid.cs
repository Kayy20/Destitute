using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointGrid
{
    public List<Vector2>[] Cells;

    private Vector2 m_cellSize;
    private Vector2 m_gridSize;
    private int m_cellsPerSide;

    public PointGrid(int cellsPerSide, Vector2 gridSize)
    {
        Cells = new List<Vector2>[cellsPerSide * cellsPerSide];
        m_cellSize = gridSize / cellsPerSide;
        m_gridSize = gridSize;
        m_cellsPerSide = cellsPerSide;
    }

    // Adds a point to a bin of the grid
    public void AddPoint(Vector2 newPoint)
    {
        int rowIndex = (int)(0.99f * m_cellsPerSide * newPoint.y / m_gridSize.y); // i
        int columnIndex = (int)(0.99f * m_cellsPerSide * newPoint.x / m_gridSize.x); // j

        int binIndex = 0; // b

        if (rowIndex % 2 == 0)
        {
            binIndex = rowIndex * m_cellsPerSide + columnIndex + 1;
        }
        else
        {
            binIndex = (rowIndex + 1) * m_cellsPerSide - columnIndex;
        }

        binIndex--; // zero-based index

        if (Cells[binIndex] == null)
        {
            Cells[binIndex] = new List<Vector2>();
        }

        Cells[binIndex].Add(newPoint);
    }





}
