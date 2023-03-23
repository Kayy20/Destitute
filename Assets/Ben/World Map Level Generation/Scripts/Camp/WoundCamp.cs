using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoundCamp : MonoBehaviour
{

    [SerializeField] GameObject map;
    public GameObject deleteButton;
    public GameObject woundButton;
    public GameObject continueButton;
    public GameObject woundParent;
    public GameObject woundImageObj;
    public Sprite woundImage;
    public List<GameObject> woundImages;


    public GameObject vert1, vert2;

    bool deletingItems, curingWounds, woundCured;

    public void OnClickContinue()
    {
        woundCured = false;
        deletingItems = false;
        curingWounds = false;

        vert1.SetActive(true);
        vert2.SetActive(false);

        InventoryController.Instance.ToggleCampUI(false);

        map.SetActive(true);
        PlayerMap player = map.GetComponentInChildren<PlayerMap>();
        player.ContinueAfterCheck(PlayerMap.StopStation.Camp);

        gameObject.SetActive(false);

    }



    public void DeleteItemClick()
    {

        if (!deletingItems && !curingWounds)
        {
            deleteButton.GetComponentInChildren<Text>().text = "Confirm Selection";
            woundButton.GetComponentInChildren<Text>().text = "Cancel Selection";
            deletingItems = true;
            continueButton.SetActive(false);

        }
        else
        {
            // Confirm Selection

            InventoryController.Instance.DeleteCampItems();
            // Refresh inventory
            InventoryController.Instance.ToggleCampUI(false);
            InventoryController.Instance.ToggleCampUI(true);
            deleteButton.GetComponentInChildren<Text>().text = "Delete Items";
            woundButton.GetComponentInChildren<Text>().text = "Cure Wounds";
            deletingItems = false;
            curingWounds = false;
            continueButton.SetActive(true);
        }

    }

    public void CureWoundClick()
    {

        if (!curingWounds && !deletingItems)
        {
            deleteButton.GetComponentInChildren<Text>().text = "Confirm Selection";
            woundButton.GetComponentInChildren<Text>().text = "Cancel Selection";
            curingWounds = true;
            continueButton.SetActive(false);
        }
        else
        {
            // Cancel Selection
            
            //InventoryController.Instance.ClearCampItems();
            
            deleteButton.GetComponentInChildren<Text>().text = "Delete Items";
            woundButton.GetComponentInChildren<Text>().text = "Cure Wounds";
            curingWounds = false;
            deletingItems = false;
            continueButton.SetActive(true);
        }

    }

    public void UpdateWoundImages(bool updateStatic = false)
    {
        if (updateStatic)
        {
            for (int i = 0; i < 3; i++)
            {
                Destroy(woundImages[i]);
            }

            woundImages.Clear();

            StaticClassVariables.Wounds -= 1;

        }
        

        for (int i = 0; i < StaticClassVariables.Wounds; i++)
        {
            // draw stuff
            GameObject g = Instantiate(woundImageObj, woundParent.transform);
            g.GetComponent<Image>().sprite = woundImage;
            woundImages.Add(g);
        }
    }

    // For wound curing
    public bool SelectedItem(ItemSO item)
    {
        if (curingWounds)
        {
            if (item.name.Equals("MedPack"))
            {
                if (!woundCured)
                {
                    // cure wound - !! only one wound cured per round !!
                    UpdateWoundImages(true);                    
                    woundCured = true;
                    return true;
                }
            }
        }

        return false;
    }

}
