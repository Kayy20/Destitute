using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    private InventoryItem currentItem;
    private Vector2 currentItemOffset;
    private List<InventoryGridTile> allTiles = new List<InventoryGridTile>();
    private bool isShowing;

    public ScaleInventoryContainer pickupInv;
    [SerializeField] private ScaleInventoryContainer playerInv;
    
    private List<ItemSpotPair> pickupPairs = new List<ItemSpotPair>();

    [SerializeField] private GameObject uiGameObjects;
    [SerializeField] private bool isPlayerInventory;

    [SerializeField] private AudioSoundSO openSound;
    [SerializeField] private AudioSoundSO closeSound;
    [SerializeField] private AudioSoundSO inventoryClick;

    [SerializeField] private GameObject baseInventoryItem;

    [SerializeField] private GameObject tooltipGO;
    [SerializeField] private Text tooltipText;

    private List<ItemSpotPair> pairs = new List<ItemSpotPair>();

    private List<GameObject> itemGOs = new List<GameObject>();

    public InventoryItem currentHoverItem;

    public List<InventoryItem> currentSelectedItems;

    public InventoryItem campFoodItem;
    public InventoryItem campDrinkItem;

    [Serializable]
    public class ItemSpotPair
    {
        public int xPos;
        public int yPos;
        public int itemID;
        public bool isRotated;
    }

    public bool IsShowing => isShowing;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowToolTipText(string s, Vector3 pos)
    {
        tooltipGO.transform.position = pos;
        tooltipGO.SetActive(true);
        tooltipText.text = s;
    }

    public void HideToolTip()
    {
        tooltipGO.SetActive(false);
    }

    public void SetCampDrink(InventoryItem item)
    {
        if (campDrinkItem != null)
        {
            campDrinkItem.UnHighlightItem();
        }

        campDrinkItem = item;
    }

    public void SetCampFood(InventoryItem item)
    {
        if (campFoodItem != null)
        {
            campFoodItem.UnHighlightItem();
        }

        campFoodItem = item;
    }

    public void DeleteCampItems() {
        if (campFoodItem != null)
        {
            RemoveFromPairs(campFoodItem);
        }

        if (campDrinkItem != null)
        {
            RemoveFromPairs(campDrinkItem);
        }
    }

    public void DeleteSelectedItems() {

        foreach (InventoryItem i in currentSelectedItems) {

            string itemName = i.ItemData.name;
            switch (itemName) {

                case "Mosin":
                case "Machette":
                case "Knife":
                    break;
                default:
                    RemoveFromPairs(i);
                    break;
            }


        }

        SaveInventory();
        currentSelectedItems.Clear();
    }

    public void DeleteItem(InventoryItem i)
    {
        RemoveFromPairs(i);
    }

    public void DeleteInMenus(InventoryItem i)
    {
        RemoveFromPairs(i);
        i.PickUpItem();
        Destroy(i.gameObject);
        
    }

    public void ClearSelectedItems() {

        foreach (InventoryItem currentSelectedItem in currentSelectedItems)
        {
            currentSelectedItem.UnHighlightItem();
        }
        currentSelectedItems.Clear();
    }

    public bool RemoveRandomItem() {

        if (pairs.Count <= 0) {
            return false;
        }
        ItemSpotPair toRemove = pairs[0];

        if (toRemove != null)
        {
            pairs.Remove(toRemove);
            SaveInventory();
        }

        return true;
    }

    public bool RemoveRandomFoodItem() {

        if (pairs.Count <= 0)
        {
            return false;
        }
        ItemSpotPair toRemove = null;

        foreach (ItemSpotPair itemSpotPair in pairs)
        {
            foreach (ItemSO iso in ItemDatabase.instance.allItems) {
                if (itemSpotPair.itemID == iso.itemID)
                {
                    if (iso.resType == AnswerType.ResourceType.Food || iso.resType == AnswerType.ResourceType.Meat)
                    {
                        CD.Log(CD.Programmers.DANIEL, $"FOUND A MATCHING ID AND SPOT!");
                        toRemove = itemSpotPair;
                        goto DeleteFood;
                        break;
                    }

                }
            }

        }

    DeleteFood:
        if (toRemove != null)
        {
            pairs.Remove(toRemove);

            SaveInventory();
            
            return true;
        }

        return false;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "Title Screen")
        {
            SaveSystem.DeleteInventory();
        }

        uiGameObjects.SetActive(true);
        /*
        foreach (InventoryGridTile tile in pickupInv.AllTiles)
        {
            allTiles.Add(tile);
        }
        */

        foreach (InventoryGridTile tile in playerInv.AllTiles)
        {
            allTiles.Add(tile);
        }



        uiGameObjects.SetActive(false);

        StartCoroutine(LoadInventory());
    }

    public void AddPickupTiles()
    {
        foreach (InventoryGridTile tile in pickupInv.AllTiles)
        {
            allTiles.Add(tile);
        }
    }

    public void RemovePickupTiles()
    {
        if (pickupInv.AllTiles == null)
            return;
        foreach (InventoryGridTile tile in pickupInv.AllTiles)
        {
            allTiles.Remove(tile);
        }
    }

    public bool PickUpInventoryItem(InventoryItem item, Vector2 offset)
    {
        if (currentItem != null)
        {
            return false;
        }

        currentItemOffset = offset;
        currentItem = item;

        RemoveFromPairs(item);

        return true;
    }

    public void RemoveFromPairs(InventoryItem item)
    {
        ItemSpotPair toRemove = null;
        foreach (ItemSpotPair itemSpotPair in pairs)
        {
            //
            if (itemSpotPair.itemID == item.ItemData.itemID)
            {
                if (itemSpotPair.xPos == item.x && itemSpotPair.yPos == item.y)
                {
                    CD.Log(CD.Programmers.DANIEL, $"FOUND A MATCHING ID AND SPOT!");
                    toRemove = itemSpotPair;
                    break;
                }

            }
        }

        if (toRemove != null)
        {
            CD.Log(CD.Programmers.DANIEL, $"removed {item.ItemData.name}", Color.magenta);
            pairs.Remove(toRemove);
            //SaveInventory();
        }
    }

    private void AddToPairs(InventoryItem item, InventoryGridTile tile, bool pickup = false)
    {

        ItemSpotPair isp = new ItemSpotPair();
        isp.xPos = tile.xPos;
        isp.yPos = tile.yPos;
        isp.itemID = item.ItemData.itemID;
        isp.isRotated = item.isRotated;

        CD.Log(CD.Programmers.DANIEL, $"added {item.ItemData.name} at {tile.xPos}, {tile.yPos}", Color.green);

        if (pickup)
        {
            pickupPairs.Add(isp);
        }
        else
        {
            pairs.Add(isp);
        }

        

        SaveInventory();
    }

    private void Update()
    {
        if (EndScreen.Instance)
        {
            if (EndScreen.Instance.isActive)
            {
                return;
            }
        }

        if (isPlayerInventory && PauseManager.instance != null)
        {
            if (Input.GetKeyDown(KeyCode.Tab) && currentItem == null && !PauseManager.instance.isPaused)
            {
                ToggleUI(!isShowing);
            }

            if (Input.GetKeyDown(KeyCode.X)&& !PauseManager.instance.isPaused)
            {
                DropCurrentItem(MovementController.Instance.gameObject.transform);
                InventoryController.Instance.HideToolTip();
                InventoryController.Instance.currentHoverItem = null;
            }

            if (Input.GetKeyDown(KeyCode.R)&& !PauseManager.instance.isPaused)
            {
                if (currentItem != null)
                {
                    currentItem.isRotated = !currentItem.isRotated;
                    currentItem.RotateItem();

                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.X) && PlayerMap.Instance.currentStation != PlayerMap.StopStation.EncounterBefore)
            {
                if (currentHoverItem != null)
                {
                    DeleteInMenus(currentHoverItem);
                    InventoryController.Instance.HideToolTip();
                    InventoryController.Instance.currentHoverItem = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.R) && PlayerMap.Instance.currentStation != PlayerMap.StopStation.EncounterBefore)
            {
                if (currentItem != null)
                {
                    currentItem.isRotated = !currentItem.isRotated;
                    currentItem.RotateItem();

                }
            }
        }

        if (currentItem != null)
        {
            currentItem.transform.position = Input.mousePosition + (Vector3)currentItemOffset;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (currentItem != null)
            {
                InventoryGridTile potentialTile = currentItem.ClosestTile(allTiles);
                if (potentialTile != null)
                {
                    InventoryItem potentialItem = currentItem;
                    if (IsValidPlacement(potentialItem, potentialTile))
                    {
                        SoundController.PlaySound(inventoryClick);
                        PlaceCurrentItem(potentialTile);
                        FillSpots(potentialItem, potentialTile);
                        AddToPairs(potentialItem, potentialTile);

                    }
                    else
                    {
                        CD.Log(CD.Programmers.DANIEL, $"invalid placement", Color.red);
                    }
                }
            }
        }
    }

    public InventoryGridTile SpecificTile(int x, int y)
    {
        foreach (InventoryGridTile inventoryGridTile in allTiles)
        {
            if (inventoryGridTile.xPos == x && inventoryGridTile.yPos == y)
            {
                return inventoryGridTile;
            }
        }

        return null;

    }

    public bool PlaceCurrentItem(InventoryGridTile tile)
    {
        if (tile.IsFull)
        {
            return false;
        }

        //tile.parentContainer.PlaceItem(currentItem);

        currentItem.transform.position = tile.gameObject.transform.position; // + new Vector3(-100,-100,0);
        currentItem.PlaceItem(tile.xPos, tile.yPos);
        currentItem = null;

        return true;
    }

    public bool HasMap()
    {
        foreach (ItemSpotPair pair in pairs) {
            ItemSO iso = ItemDatabase.ItemFromID(pair.itemID);
            if (iso.name == "Map")
            {
                return true;
            }
        }

        return false;
    }

    public bool PlaceItem(InventoryItem item, InventoryGridTile tile)
    {
        //CD.Log(CD.Programmers.DANIEL, $"trying to place {item.ItemData.name}",Color.black);
        if (tile.IsFull)
        {
            CD.Log(CD.Programmers.DANIEL, $"cant place, {tile.xPos},{tile.yPos} is full", Color.red);
            return false;
        }

        /*
        tile.parentContainer.PlaceItem(item);
        item.transform.position = tile.gameObject.transform.position; // + new Vector3(-100,-100,0);
        item.PlaceItem();
        */

        item.transform.position = tile.gameObject.transform.position; // + new Vector3(-100,-100,0);
        item.PlaceItem(tile.xPos, tile.yPos);

        Debug.Log("SETTING PLACES 1");
        item.x = tile.xPos;
        item.y = tile.yPos;

        //CD.Log(CD.Programmers.DANIEL, $"successfully placed {item.ItemData.name}", Color.gray);
        return true;
    }

    public bool IsValidPlacement(InventoryItem item, InventoryGridTile tile)
    {

        //CD.Log(CD.Programmers.DANIEL, $"checking if {item} can be placed at {tile}");
        int startingX = tile.xPos;
        int startingY = tile.yPos;

        int itemEndX = startingX + item.width;
        int itemEndY = startingY + item.height;

        if (itemEndX > tile.parentContainer.Width || itemEndY > tile.parentContainer.Height)
        {
            return false;
        }

        for (int i = startingX; i < itemEndX; i++)
        {
            for (int j = startingY; j < itemEndY; j++)
            {
                InventoryGridTile tileToCheck = tile.parentContainer.AllTiles[j, i];
                if (tileToCheck.IsFull)
                {
                    CD.Log(CD.Programmers.DANIEL, $"{tileToCheck} is full");
                    return false;
                }
            }
        }

        return true;
    }

    public bool IsValidPlacement(ItemSO item, InventoryGridTile tile)
    {
        int startingX = tile.xPos;
        int startingY = tile.yPos;

        int itemEndX = startingX + item.width;
        int itemEndY = startingY + item.height;

        if (itemEndX > tile.parentContainer.Width || itemEndY > tile.parentContainer.Height)
        {
            return false;
        }

        for (int i = startingX; i < itemEndX; i++)
        {
            for (int j = startingY; j < itemEndY; j++)
            {
                InventoryGridTile tileToCheck = tile.parentContainer.AllTiles[j, i];
                if (tileToCheck.IsFull)
                    return false;
            }
        }

        return true;
    }

    public void FillSpots(InventoryItem item, InventoryGridTile tile)
    {
        //THE COORDS ARE IN X,Y
        int startingX = tile.xPos;
        int startingY = tile.yPos;

        int itemEndX = startingX + item.width;
        int itemEndY = startingY + item.height;

        for (int i = startingX; i < itemEndX; i++)
        {
            for (int j = startingY; j < itemEndY; j++)
            {
                InventoryGridTile tileToSet = tile.parentContainer.AllTiles[j, i];
                //CD.Log(CD.Programmers.DANIEL, $"filling {j}, {i}", Color.blue);
                item.effectedTiles.Add(tileToSet);
                tileToSet.SetFull(true);
            }
        }
    }

    public void ClearPickup()
    {
        pickupPairs.Clear();
    }



    public bool SpawnObject(ItemSO item, [CanBeNull] ScaleInventoryContainer container)
    {
        CD.Log(CD.Programmers.DANIEL, item.name.ToString());

        ScaleInventoryContainer curContainer = playerInv;
        if (container != null)
        {
            curContainer = container;
        }

        GameObject go = Instantiate(baseInventoryItem, new Vector3(-999, -999, -999), Quaternion.identity, curContainer.transform.parent);

        InventoryItem invItem = go.GetComponent<InventoryItem>();

        invItem.SetValues(item);

        itemGOs.Add(go);

        foreach (InventoryGridTile tile in curContainer.AllTiles)
        {
            if (IsValidPlacement(invItem, tile))
            {
                //CD.Log(CD.Programmers.DANIEL, $"PLACING AT {tile}");
                PlaceItem(invItem, tile);
                FillSpots(invItem, tile);
                if (curContainer == pickupInv)
                {
                    AddToPairs(invItem, tile, true);
                }
                else
                {
                    AddToPairs(invItem, tile);
                }
                

                
                //SaveInventory();
                return true;
            }
        }
        invItem.RotateItem();
        invItem.isRotated = true;
        foreach (InventoryGridTile tile in curContainer.AllTiles)
        {
            if (IsValidPlacement(invItem, tile))
            {
                //CD.Log(CD.Programmers.DANIEL, $"PLACING AT {tile}");
                PlaceItem(invItem, tile);
                FillSpots(invItem, tile);
                if (curContainer == pickupInv)
                {
                    AddToPairs(invItem, tile, true);
                }
                else
                {
                    AddToPairs(invItem, tile);
                }


                //SaveInventory();
                return true;
            }
        }


        itemGOs.Remove(go);
        Destroy(go);
        return false;
    }

    public InventoryItem CreateItem(ItemSO item)
    {
        ScaleInventoryContainer curContainer = playerInv;

        GameObject go = Instantiate(baseInventoryItem, new Vector3(-999, -999, -999), Quaternion.identity, curContainer.transform.parent);

        go.name = "Inventory item" + Random.Range(0, 9999);
        InventoryItem invItem = go.GetComponent<InventoryItem>();

        invItem.SetValues(item);

        return invItem;
    }

    public void ToggleUI(bool value)
    {
        HideToolTip();
        uiGameObjects.SetActive(value);
        isShowing = value;

        if (isShowing)
        {
            if (MovementController.Instance != null)
            {
                MovementController.Instance.enableInput(false);
            }

            SoundController.PlaySound(openSound);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;

            StartCoroutine(LoadInventory());
        }
        else
        {
            if (MovementController.Instance != null)
            {
                MovementController.Instance.enableInput(true);
            }

            SoundController.PlaySound(closeSound);

            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;

            SaveInventory();
        }
    }

    // This is for reloading back cuz some reason doesn't like to find ui again
    public void SetInstanceAgain(InventoryController uiSet)
    {
        Instance = uiSet;
    }

    public void ToggleCampUI(bool value)
    {
        uiGameObjects.SetActive(value);
        isShowing = value;

        if (isShowing)
        {

            SoundController.PlaySound(openSound);

            StartCoroutine(LoadInventory());
        }
        else
        {
            SoundController.PlaySound(closeSound);

            SaveInventory();
        }
    }

    public void SaveInventory()
    {
        //CD.Log(CD.Programmers.DANIEL, $"ITEMS BEING SAVED: {pairs.Count}", Color.green);
        SaveSystem.SaveInventory(pairs);
    }

    public IEnumerator LoadInventory()
    {
        SaveSystem.InventorySaveData data = SaveSystem.LoadInventory();

        if (data == null)
        {
            yield break;
        }

        foreach (GameObject itemGO in itemGOs)
        {
            Destroy(itemGO);
        }

        playerInv.ClearAllSpots();

        pairs = data.Pairs;

        //CD.Log(CD.Programmers.DANIEL, $"ITEMS IN INVENTORY: {pairs.Count}", Color.magenta);
        CD.Log(CD.Programmers.DANIEL, "STARTING LOADING INVENTORY", Color.magenta);
        foreach (ItemSpotPair itemSpotPair in pairs)
        {

            CD.Log(CD.Programmers.DANIEL, $"ITEM ID: {itemSpotPair.itemID}, X pos: {itemSpotPair.xPos} Y pos: {itemSpotPair.yPos}, ROT: {itemSpotPair.isRotated}", Color.yellow);

            InventoryGridTile tileToPlaceAt = SpecificTile(itemSpotPair.xPos, itemSpotPair.yPos);

            InventoryItem item = CreateItem(ItemDatabase.ItemFromID(itemSpotPair.itemID));

            item.SetRotation(itemSpotPair.isRotated);

            yield return new WaitForEndOfFrame();



            PlaceItem(item, tileToPlaceAt);
            FillSpots(item, tileToPlaceAt);

            itemGOs.Add(item.gameObject);
        }
    }

    public void DropCurrentItem(Transform t)
    {
        if (currentHoverItem == null) return;
        currentHoverItem.PickUpItem();
        GameObject gItem = Instantiate(currentHoverItem.ItemData.prefab, t.position + t.forward + t.up, quaternion.identity, null);

        RemoveFromPairs(currentHoverItem);
        Destroy(currentHoverItem.gameObject);
        currentHoverItem = null;
    }

    public int RemoveValuables()
    {
        CD.Log(CD.Programmers.DANIEL, $"pairs length: {pairs.Count}");
        
        
        int num = 0;
        List<ItemSpotPair> toRemove = new List<ItemSpotPair>();
        
        
        
        foreach (ItemSpotPair pair in pairs) {
            ItemSO iso = ItemDatabase.ItemFromID(pair.itemID);
            if (iso.resType == AnswerType.ResourceType.Valuable) {
                num += iso.height * iso.width;
                CD.Log(CD.Programmers.DANIEL, $"ADDING TO REMOVE {iso.name} , {iso.itemID}");
                toRemove.Add(pair);
            }
        }

        
        foreach (ItemSpotPair p in toRemove) {
            CD.Log(CD.Programmers.DANIEL, $"REMOVING {p.itemID}");
            pairs.Remove(p);
        }
        

        SaveInventory();
        return num;
    }
}