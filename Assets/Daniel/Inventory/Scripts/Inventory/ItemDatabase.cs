using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : MonoBehaviour
{
    public static ItemDatabase instance;
    public List<ItemSO> allItems;

    private void Awake()
    {
        instance = this;
    }

    public static ItemSO ItemFromID(int id)
    {
        foreach (ItemSO allItem in instance.allItems)
        {
            if (allItem.itemID == id)
            {
                return allItem;
            }
        }

        return null;
    }
}
