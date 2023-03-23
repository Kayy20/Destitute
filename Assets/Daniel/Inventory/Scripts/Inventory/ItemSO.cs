using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/NewItem", order = 1)]
public class ItemSO : ScriptableObject
{
    public enum ItemRarity
    {
        Common, Uncommon, Rare
    }

    public string name;
    public int itemID;
    public Sprite image;
    public GameObject prefab;
    public ItemRarity rarity;
    public AnswerType.ResourceType resType;
    public int width;
    public int height;
    //public InventoryItem inventoryData;
    
}
