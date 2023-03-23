using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager instance;
    public static BuildingManager Instance
    {
        get 
        { 
            if (instance != null) 
                return instance;
            
            Debug.LogError("NO BUILDING MANAGER IN SCENE. PLEASE PLACE A BUILDING MANAGER INTO THE SCENE FROM THE PREFABS FOLDER FOR BUILDING SPAWNERS TO FUNCTION.");
            return null;
        }
    }

    List<SO_House> houses;
    List<SO_Shop> shops;

    bool foodShop = false;
    bool medicalShop = false;
    bool survivalShop = false;

    private void Awake()
    {
        instance = this;

        //populate lists
        Resources.LoadAll<SO_Building>("Houses");
        SO_House[] houseArray = Resources.FindObjectsOfTypeAll(typeof(SO_House)) as SO_House[];
        SO_Shop[] shopArray = Resources.FindObjectsOfTypeAll(typeof(SO_Shop)) as SO_Shop[];


        houses = new List<SO_House>();
        shops = new List<SO_Shop>();

        for (int i = 0; i < houseArray.Length; i++)
            houses.Add(houseArray[i]);

        for (int i = 0; i < shopArray.Length; i++)
            shops.Add(shopArray[i]);

        foodShop = false;
        medicalShop = false;
        survivalShop = false;
    }

    public GameObject GetBuilding(bool isShop)
    {

        if (isShop)
        {
            return GetShop();
        }
        else
        {
            return houses[Random.Range(0, houses.Count)].BuildingPrefab;

        }

        
    }

    public GameObject GetShop()
    {
        if(foodShop && medicalShop && survivalShop)
        {
            foodShop = false;
            medicalShop = false;
            survivalShop = false;
        }
        
        SO_Shop shopSO;

        do
        {
            shopSO = shops[Random.Range(0, shops.Count)];

        } while (shopTypeAvailible(shopSO));

        GameObject shop =shopSO.BuildingPrefab;

        switch (shopSO.sType)
        {
            case SO_Shop.ShopType.Food:
                foodShop = true;
                break;

            case SO_Shop.ShopType.Medical:
                medicalShop = true;
                break;

            case SO_Shop.ShopType.Survival:
                survivalShop = true;
                break;
        }

        return shop;
    }

    public GameObject GetShop(int sType)
    {
        SO_Shop shopSO;

        do
        {
            shopSO = shops[Random.Range(0, shops.Count)];

        } while (shopTypeMatches(sType,shopSO));

        GameObject shop = shopSO.BuildingPrefab;

        return shop;

    }

    bool shopTypeMatches(int st,SO_Shop s)
    {
        switch (s.sType)
        {
            case SO_Shop.ShopType.Food:
                if (st==0)
                    return true;
                return false;
            case SO_Shop.ShopType.Medical:
                if (st==1)
                    return true;
                return false;
            case SO_Shop.ShopType.Survival:
                if (st==2)
                    return true;
                return false;
        }
        Debug.LogError("BROKEN AF");
        return true;
    }

    bool shopTypeAvailible(SO_Shop s)
    {
        switch (s.sType)
        {
            case SO_Shop.ShopType.Food:
                if (foodShop)
                    return true;
                return false;
            case SO_Shop.ShopType.Medical:
                if (medicalShop)
                    return true;
                return false;
            case SO_Shop.ShopType.Survival:
                if (survivalShop)
                    return true;
                return false;
        }

        Debug.LogError("BROKEN AF");
        return true;
    }
}
