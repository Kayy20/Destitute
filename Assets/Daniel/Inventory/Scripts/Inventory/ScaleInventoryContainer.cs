using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScaleInventoryContainer : MonoBehaviour
{
    [SerializeField] private RectTransform parentRect;
    [SerializeField] private int gridWidth;
    [SerializeField] private int gridHeight;

    [SerializeField] private Sprite bgSprite;

    //[SerializeField] private int spacing;
    private InventoryGridTile[,] gridTiles;

    private GridLayoutGroup gridGroup;
    private Vector2 cellSize;

    public InventoryGridTile[,] AllTiles => gridTiles;
    public int Width => gridWidth;
    public int Height => gridHeight;

    private void Awake()
    {
        gridGroup = GetComponent<GridLayoutGroup>();
        gridTiles = new InventoryGridTile[gridWidth, gridHeight];

        float width = (parentRect.rect.width -20) / gridWidth;
        float height = (parentRect.rect.height -20) / gridHeight;

        cellSize = new Vector2(width , height);

        gridGroup.cellSize = cellSize;

        SpawnGridVisuals();
    }

    private void SpawnGridVisuals()
    {
        for (int i = gridWidth-1; i >= 0; i--)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                GameObject vis = new GameObject();
                InventoryGridTile tile = vis.AddComponent<InventoryGridTile>();
                
                vis.name = $"Tile[{i},{j}]";
                Image img = vis.AddComponent<Image>();
                img.sprite = bgSprite;
                tile.parentContainer = this;
                tile.xPos = j;
                tile.yPos = i;
                vis.transform.SetParent(parentRect.transform);

                RectTransform rt = vis.GetComponent<RectTransform>();
                rt.pivot = Vector2.zero;
                gridTiles[i, j] = tile;
            }
        }
    }

    public void PlaceItem(InventoryItem item)
    {
        /*
        if (item.ItemData.resType == AnswerType.ResourceType.Medical)
        {
            //CD.Log(CD.Programmers.DANIEL, "Placed A Medical Item", Color.green);
        }
        */

    }

    public void ClearAllSpots()
    {
        foreach (InventoryGridTile inventoryGridTile in gridTiles)
        {
            inventoryGridTile.SetFull(false);
        }
    }
}