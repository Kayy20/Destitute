using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryGridTile : MonoBehaviour , IPointerEnterHandler
{
    public ScaleInventoryContainer parentContainer;
    private bool full;

    public int xPos;
    public int yPos;

    public bool IsFull => full;

    private void Start()
    {
        this.GetComponent<RectTransform>().localScale = Vector3.one;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void SetFull(bool value)
    {
        full = value;
        GetComponent<Image>().color = full ? Color.red : Color.green;
    }
}
