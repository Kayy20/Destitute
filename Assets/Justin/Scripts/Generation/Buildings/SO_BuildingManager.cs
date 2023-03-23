using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SO_BuildingManager : ScriptableObject
{

    public List<SO_House> buildings;
    public List<SO_Shop> shops;

    public GameObject GetBuilding(bool shop)
    {

        if (shop)
        {
            if (shops.Count > 0)
                return shops[Random.Range(0, shops.Count)].BuildingPrefab;

            Debug.LogError("NO SHOPS HAVE BEEN MADE");
            return null;
            
        }
        else
        {
            if (buildings.Count > 0)
                return buildings[Random.Range(0, buildings.Count)].BuildingPrefab;

            Debug.LogError("NO BUILDINGS HAVE BEEN MADE");
            return null;
        }
    }

}
