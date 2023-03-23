using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemPool : MonoBehaviour
{
    public static ItemPool Instance;
    
    public List<InventoryItem> items;

    private void Awake()
    {
        Instance = this;
    }

    public InventoryItem RandomItem()
    {
        int rand = Random.Range(0, items.Count);

        InventoryItem outItem = items[rand];

        items.Remove(outItem);
        
        return outItem;
    }
}
