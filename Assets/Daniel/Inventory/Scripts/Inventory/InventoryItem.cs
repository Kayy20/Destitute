using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IPointerUpHandler
{
    public List<InventoryGridTile> effectedTiles = new List<InventoryGridTile>();
    private RectTransform rt;
    private Image img;

    [SerializeField] private GameObject visualGO;

    private ItemSO itemData;

    public int width;
    public int height;

    public int x;
    public int y;

    public bool isRotated;

    private bool inPickup;

    public ItemSO ItemData => itemData;

    private Vector2 startClickPos;
    private void Awake()
    {
        rt = GetComponent<RectTransform>();
        img = visualGO.GetComponent<Image>();
    }

    public void SetValues(ItemSO item)
    {

        CD.Log(CD.Programmers.DANIEL, item.name.ToString());

        rt = GetComponent<RectTransform>();
        img = visualGO.GetComponent<Image>();

        itemData = item;
        
        width = item.width;
        height = item.height;

        //CD.Log(CD.Programmers.DANIEL, $"setting image to {item.image.name}");
        img.sprite = item.image;

        itemData = item;
        rt.sizeDelta = new Vector2(100 * width, 100 * height);
        visualGO.GetComponent<RectTransform>().sizeDelta = new Vector2(100 * width, 100 * height);

    }

    public void SetRotation(bool val)
    {
        isRotated = val;
        if (isRotated)
        {
            RotateItem();
        }
    }

    public void PlaceItem(int inX, int inY)
    {
        x = inX;
        y = inY;
        SetRaycastState(true);
    }

    private void SetRaycastState(bool value)
    {
        img.raycastTarget = value;
    }

    public void PickUpItem()
    {
        foreach (InventoryGridTile inventoryGridTile in effectedTiles)
        {
            inventoryGridTile.SetFull(false);
        }
        
        effectedTiles.Clear();
    }

    public void RotateItem()
    {
        int prevW = width;
        int prevH = height;

        width = prevH;
        height = prevW;
        rt.sizeDelta = new Vector2(100 * width, 100 * height);
        
        Vector3 rot = Vector3.zero;
        if (isRotated)
        {
            rot.z = 90;
            visualGO.GetComponent<RectTransform>().sizeDelta = new Vector2(100 * height, 100 * width);
        }
        else
        {
            visualGO.GetComponent<RectTransform>().sizeDelta = new Vector2(100 * width, 100 * height);
        }
        
        visualGO.GetComponent<RectTransform>().eulerAngles = rot;
        
        
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        inPickup = false;
        if (Vector2.Distance(startClickPos, Input.mousePosition) > 10)
        {
            return;
        }

        if(PlayerMap.Instance.currentStation == PlayerMap.StopStation.Camp){

            if(itemData.resType == AnswerType.ResourceType.Food || ItemData.resType == AnswerType.ResourceType.Meat){
                CampController.Instance.SelectedItem(itemData);
                InventoryController.Instance.SetCampFood(this);
                HighlightItem();
            }
            else if(itemData.resType == AnswerType.ResourceType.Water){
                CampController.Instance.SelectedItem(itemData);
                InventoryController.Instance.SetCampDrink(this);
                HighlightItem();
            }

        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("clicked");

        if (Input.GetMouseButton(0))
        {
            /*
            if (SceneManager.GetActiveScene().name == "MapScene")
            {
                CampController.Instance.SelectedItem(itemData);
                InventoryController.Instance.currentSelectedItems.Add(this);
                return;
            }
            */
            
            startClickPos = Input.mousePosition;
            inPickup = true;
            
            if (PlayerMap.Instance)
            {
                if(PlayerMap.Instance.currentStation == PlayerMap.StopStation.EncounterBefore || PlayerMap.Instance.currentStation == PlayerMap.StopStation.EncounterAfter){

                    if(EncounterUI.Instance.ItemSelected(itemData)){
                        InventoryController.Instance.currentSelectedItems.Add(this);
                        HighlightItem();
                    }
                    return;
                }

                if (PlayerMap.Instance.currentStation == PlayerMap.StopStation.Camp)
                {
                    if (itemData.name == "MedPack" && !Camp.Instance.woundCured)
                    {
                        if (StaticClassVariables.Wounds > 0)
                        {
                            StaticClassVariables.Wounds--;
                    
                            InventoryController.Instance.DeleteItem(this);
                            Debug.Log("DESTROYING MEDPACK");
                            Destroy(this.gameObject);
                            Camp.Instance.UpdateWoundImages(true);
                            
                            InventoryController.Instance.HideToolTip();
                            InventoryController.Instance.currentHoverItem = null;
                        }
                    }
                }
            }

            
            
        }
    }

    void Update()
    {

        if (inPickup && Vector2.Distance(startClickPos, Input.mousePosition) > 10)
        {
            if (InventoryController.Instance.PickUpInventoryItem(this, transform.position - Input.mousePosition))
            {
                
                SetRaycastState(false);
                PickUpItem();
            }
            
            inPickup = false;
        }
    }



    [CanBeNull]
    public InventoryGridTile ClosestTile(List<InventoryGridTile> tiles)
    {
        InventoryGridTile outTile = tiles[0];

        float closestDist = 99999;
        foreach (InventoryGridTile inventoryGridTile in tiles)
        {
            float dist = Vector2.Distance(
                this.transform.position, 
                inventoryGridTile.transform.position);
            if (dist < closestDist)
            {
                outTile = inventoryGridTile;
                closestDist = dist;
            }
        }

        if (closestDist > 150)
        {
            return null;
        }

        return outTile;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        InventoryController.Instance.currentHoverItem = this;
        InventoryController.Instance.ShowToolTipText(itemData.name, this.transform.position + new Vector3(width *50, height * 50,0));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (InventoryController.Instance.currentHoverItem == this)
        {
            InventoryController.Instance.HideToolTip();
            InventoryController.Instance.currentHoverItem = null;
        }
        
    }

    public void HighlightItem()
    {
        Color c = img.color;
        c.a = 0.5f;
        img.color = c;
    }
    
    public void UnHighlightItem()
    {
        Color c = img.color;
        c.a = 1f;
        img.color = c;
    }

    
}
