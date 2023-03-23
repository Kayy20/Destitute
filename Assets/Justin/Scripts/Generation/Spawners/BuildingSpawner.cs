using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawner : CustomMonoBehaviour
{

    [SerializeField]
    private bool isShop;
    public bool IsShop
    {
        get { return isShop; }
    }
    [HideInInspector]
    public bool debugMode;

    [HideInInspector]
    public bool hasBuildingOverride;
    [HideInInspector]
    public GameObject buildingOverride;

    [HideInInspector]
    public bool hasShopTypeOverride;
    [HideInInspector]
    public int shopTypeOverride=0;

    private void Start()
    {

        if (debugMode)
        {
            if (hasBuildingOverride)
            {
                GameObject.Instantiate(buildingOverride, transform.position, transform.rotation, transform);

            }
            else if (hasShopTypeOverride)
            {
                GameObject.Instantiate(BuildingManager.Instance.GetShop(shopTypeOverride), transform.position, transform.rotation, transform);

            }
            else
            {
                GameObject.Instantiate(BuildingManager.Instance.GetBuilding(isShop), transform.position, transform.rotation, transform);
            }
        }
        else
        {
            GameObject spawnedGO=GameObject.Instantiate(BuildingManager.Instance.GetBuilding(isShop), transform.position, transform.rotation, transform);

            if (isShop)
            {
                foreach(ItemSpawner s in spawnedGO.GetComponentsInChildren<ItemSpawner>())
                {
                    s.SetAsShopSpawner();
                }
            }

        }
    }

}
