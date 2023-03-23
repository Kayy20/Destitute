using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerItemInteraction : MonoBehaviour
{
    [SerializeField] private float interactionDistance;
    
    private PickupItem currentItem;

    public bool CanInteract;

    public UnityEvent PickupEvent;
    
    void Update()
    {
        if(InventoryController.Instance.IsShowing)
            return;
        
        RaycastHit hit;
        //raycast
        if (Physics.Raycast(transform.position, transform.forward, out hit, Mathf.Infinity))
        {
            //hit its in the distance
            
            if (hit.distance <= interactionDistance)
            {
                if (hit.collider.gameObject.TryGetComponent(out PickupItem pickup))
                {
                    currentItem = pickup;
                }
                else{
                    currentItem = null;
                }
            }
        }

        if (currentItem != null)
        {
            CanInteract = true;
            if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
            {
                PickupCurrentItem();
            }
        }
        else
        {
            CanInteract = false;
        }
    }

    private void PickupCurrentItem()
    {
        CD.Log(CD.Programmers.DANIEL, $"picking up {currentItem.ItemData}");
        if (InventoryController.Instance.SpawnObject(currentItem.ItemData, null))
        {
            Destroy(currentItem.gameObject);
            PickupEvent?.Invoke();
            currentItem = null;
        }
        //InventoryController.Instance.AddItemToInventory(currentItem.ItemData);
        
        
    }
}
