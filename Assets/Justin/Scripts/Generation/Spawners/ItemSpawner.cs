using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : CustomMonoBehaviour
{
    public enum ItemType
    {
        Food,
        Water,
        Medical,
        Survival,
        Valuable
    }

    ItemType iType;

    
    [HideInInspector]
    public bool isValuable;

    //get a reference to a helper that finds and sorts all items
    [HideInInspector]
    public float FoodChance = 1.0f/4;
    [HideInInspector]
    public float WaterChance = 1.0f / 4;
    [HideInInspector]
    public float MedChance = 1.0f / 4;
    [HideInInspector]
    public float SurvivalChance = 1.0f / 4;
    [HideInInspector]
    public float valuableChance = 0.05f;

    private float[] lastVars = {1.0f / 4, 1.0f / 4, 1.0f / 3, 1/4.0f };
    public float[] LastVars
    {
        get { return lastVars; }
    }
    [SerializeField]
    private SpawnSettingsSO spawnSettings;

   [HideInInspector]
    public bool DebugMode=false;

    [HideInInspector]
    public bool hasFlatSpawnRate = false;
    [HideInInspector]
    public float flatSpawnRate = 1.0f;

    [HideInInspector]
    public bool hasSpawnOverride= false;
    [HideInInspector]
    public GameObject spawnOverride;

    [HideInInspector]
    public bool hasItemTypeOverride = false;
    [HideInInspector]
    public ItemType itemTypeOverride;

    float bonus = 0;
    private void Start()
    {
        SpawnItem();
    }

    public void SetAsShopSpawner()
    {
        bonus = 0.2f;
    }


    public void UpdateVars(int updatedVar)
    {

        float remainder;
        float ratio1,ratio2,ratio3;

        switch (updatedVar)
        {
            case 0:

                remainder = 1f - FoodChance;
                
                if(SurvivalChance >0 || MedChance > 0 || WaterChance > 0)
                {
                    ratio1 = MedChance / (SurvivalChance + MedChance + WaterChance);
                    ratio2 = SurvivalChance / (MedChance + SurvivalChance + WaterChance);
                    ratio3 = WaterChance / (MedChance + SurvivalChance + WaterChance);
                }
                else
                {
                    ratio1 = 1;
                    ratio2 = 1;
                    ratio3 = 1;
                }


                if (remainder == 0)
                {
                    MedChance = 0f;
                    SurvivalChance = 0f;
                    WaterChance = 0f;
                }
                else
                {
                    MedChance = ratio1 * remainder;
                    SurvivalChance = ratio2 * remainder;
                    WaterChance = ratio3 * remainder;
                }

                break;
            case 1:
                remainder = 1f - MedChance;

                if (SurvivalChance>0 || FoodChance>0|| WaterChance > 0)
                {
                    ratio1 = FoodChance / (SurvivalChance + FoodChance+ WaterChance);
                    ratio2 = SurvivalChance / (FoodChance + SurvivalChance+ WaterChance);
                    ratio3 = WaterChance / (FoodChance + SurvivalChance+ WaterChance);
                }
                else
                {
                    ratio1 = 1;
                    ratio2 = 1;
                    ratio3 = 1;
                 }
        

                if (remainder == 0)
                {
                    FoodChance = 0;
                    SurvivalChance = 0;
                    WaterChance = 0;
                }
                else
                {
                    FoodChance = ratio1 * remainder;
                    SurvivalChance = ratio2 * remainder;
                    WaterChance = ratio3 * remainder;
                }

              

                break;
            case 2:
                remainder = 1f - SurvivalChance;

                if (FoodChance>0||MedChance>0|| WaterChance > 0)
                {
                    ratio1 = MedChance / (FoodChance + MedChance+ WaterChance);
                    ratio2 = FoodChance / (MedChance + FoodChance+ WaterChance);
                    ratio3 = WaterChance / (MedChance + FoodChance + WaterChance);
                }
                else
                {
                    ratio1 = 1;
                    ratio2 = 1;
                    ratio3 = 1;
                }
                
                if (remainder == 0)
                {
                    MedChance = 0;
                    FoodChance = 0;
                    WaterChance = 0;
                }
                else{
                    MedChance = ratio1 * remainder;
                    FoodChance = ratio2 * remainder;
                    WaterChance = ratio3 * remainder;
                }

                break;

            case 3:
                remainder = 1f - WaterChance;

                if (FoodChance > 0 || MedChance > 0 || WaterChance > 0)
                {
                    ratio1 = MedChance / (FoodChance + MedChance + SurvivalChance);
                    ratio2 = FoodChance / (MedChance + FoodChance + SurvivalChance);
                    ratio3 = SurvivalChance / (MedChance + FoodChance + SurvivalChance);
                }
                else
                {
                    ratio1 = 1;
                    ratio2 = 1;
                    ratio3 = 1;
                }

                if (remainder == 0)
                {
                    MedChance = 0;
                    FoodChance = 0;
                    SurvivalChance = 0;
                }
                else
                {
                    MedChance = ratio1 * remainder;
                    FoodChance = ratio2 * remainder;
                    SurvivalChance = ratio3 * remainder;
                }

                break;

        }

        lastVars[0] = FoodChance;
        lastVars[1] = MedChance;
        lastVars[2] = SurvivalChance;
        lastVars[3] = WaterChance;

    }

    public void SpawnItem()
    {
        if (DebugMode)
        {
            if (hasSpawnOverride)
            {
                if (Random.Range(0, 1f) < flatSpawnRate)
                {
                    GameObject itemGO = GameObject.Instantiate(spawnOverride, transform.position, Quaternion.identity, transform);

                    Outline[] os = GetComponentsInChildren<Outline>();

                    CD.Log(CD.Programmers.JUSTIN, "Count in children: " + os.Length);
                    CD.Log(CD.Programmers.JUSTIN, "Colour: " + ItemManager.Instance.foodColour);

                    for (int i = 0; i < os.Length; i++)
                        os[i].SetColour(ItemManager.Instance.foodColour);

                    

                }

            }else if (hasItemTypeOverride)
            {
                if (hasFlatSpawnRate)
                {
                    if (Random.Range(0, 1f) < flatSpawnRate)
                        switch (itemTypeOverride)
                        {
                            case ItemType.Food:
                                SpawnFood();
                                break;
                            case ItemType.Water:
                                SpawnWater();
                                break;
                            case ItemType.Medical:
                                SpawnMedical();
                                break;
                            case ItemType.Survival:
                                SpawnSurvival();
                                break;

                        }
                }
                else
                {
                    switch (itemTypeOverride)
                    {
                        case ItemType.Food:
                            if (Random.Range(0, 1.0f) < GetFoodChance())
                                SpawnFood();
                            break;
                        case ItemType.Water:
                            if (Random.Range(0, 1.0f) < GetFoodChance())
                                SpawnWater();
                            break;
                        case ItemType.Medical:
                            if (Random.Range(0, 1.0f) < GetMedicalChance())
                                SpawnMedical();
                            break;
                        case ItemType.Survival:
                            if (Random.Range(0, 1.0f) < GetSurvivalChance())
                                SpawnSurvival();
                            break;

                    }
                }
            }

        }
        else
        {
            if (isValuable)
            {
                if (Random.Range(0, 1.0f) < spawnSettings.ValuableChance)
                    SpawnValuable();

            }
            else
            {
                float spawnedItem = Random.Range(0.0f, 1.0f);
            
           
                //determine what type of item


                if (spawnedItem <= FoodChance)
                {
                    //spawn food
                    iType = ItemType.Food;

                }
                else if (spawnedItem <= FoodChance + MedChance)
                {
                    //spawn med
                    iType = ItemType.Medical;

                }
                else if(spawnedItem <= FoodChance + MedChance+SurvivalChance)
                {
                    //spawn survival
                    iType = ItemType.Survival;

                }
                else
                {
                    iType = ItemType.Water;
                }

                //determine if spawn should happen

                switch (iType)
                {
                    case ItemType.Food:
                        if (Random.Range(0, 1.0f) < GetFoodChance()+bonus)
                            SpawnFood();
                        break;
                    case ItemType.Water:
                        if (Random.Range(0, 1.0f) < GetWaterChance() + bonus)
                            SpawnWater();
                        break;
                    case ItemType.Medical:
                        if (Random.Range(0, 1.0f) < GetMedicalChance() + bonus)
                            SpawnMedical();
                        break;
                    case ItemType.Survival:
                        if (Random.Range(0, 1.0f) < GetSurvivalChance() + bonus)
                            SpawnSurvival();
                        break;

                }

            }
        }
        
       

    }

    private void SpawnFood()
    {

        GameObject itemGO = GameObject.Instantiate(ItemManager.Instance.GetFood(), transform.position, Quaternion.identity,transform);

        Outline[] os = itemGO.GetComponentsInChildren<Outline>();

        for (int i = 0; i < os.Length; i++)
            os[i].SetColour(ItemManager.Instance.foodColour);

        os = itemGO.GetComponents<Outline>();

        for (int i = 0; i < os.Length; i++)
            os[i].SetColour(ItemManager.Instance.foodColour);
    }
    private void SpawnWater()
    {

        GameObject itemGO = GameObject.Instantiate(ItemManager.Instance.GetWater(), transform.position, Quaternion.identity, transform);

        Outline[] os = itemGO.GetComponentsInChildren<Outline>();

        for (int i = 0; i < os.Length; i++)
            os[i].SetColour(ItemManager.Instance.watColour);

        os = itemGO.GetComponents<Outline>();

        for (int i = 0; i < os.Length; i++)
            os[i].SetColour(ItemManager.Instance.watColour);
    }
    private void SpawnMedical()
    {

        GameObject itemGO = GameObject.Instantiate(ItemManager.Instance.GetMedical(), transform.position, Quaternion.identity, transform);

        Outline[] os = itemGO.GetComponentsInChildren<Outline>();

        for (int i = 0; i < os.Length; i++)
            os[i].SetColour(ItemManager.Instance.medColour);

        os = itemGO.GetComponents<Outline>();

        for (int i = 0; i < os.Length; i++)
            os[i].SetColour(ItemManager.Instance.medColour);
    }
    private void SpawnSurvival()
    {

        GameObject itemGO = GameObject.Instantiate(ItemManager.Instance.GetSurvival(), transform.position, Quaternion.identity, transform);

        Outline[] os = itemGO.GetComponentsInChildren<Outline>();

        for (int i = 0; i < os.Length; i++)
            os[i].SetColour(ItemManager.Instance.survColour);

        os = itemGO.GetComponents<Outline>();

        for (int i = 0; i < os.Length; i++)
            os[i].SetColour(ItemManager.Instance.survColour);
    }

    private void SpawnValuable()
    {
        GameObject itemGO = GameObject.Instantiate(ItemManager.Instance.GetValuable(), transform.position, Quaternion.identity, transform);

        Outline[] os = itemGO.GetComponentsInChildren<Outline>();

        for (int i = 0; i < os.Length; i++)
            os[i].SetColour(ItemManager.Instance.valColour);

        os = itemGO.GetComponents<Outline>();

        for (int i = 0; i < os.Length; i++)
            os[i].SetColour(ItemManager.Instance.valColour);
    }

    private float GetFoodChance()
    {
        return ((float)SpawnManager.FoodStar / SpawnManager.MAX_STARS) * spawnSettings.maxSpawnRate;
    }
    private float GetWaterChance()
    {
        return ((float)SpawnManager.FoodStar / SpawnManager.MAX_STARS) * spawnSettings.maxSpawnRate;
    }
    private float GetMedicalChance()
    {
        return ((float)SpawnManager.MedicalStar / SpawnManager.MAX_STARS) * spawnSettings.maxSpawnRate;
    }
    private float GetSurvivalChance()
    {
        return ((float)SpawnManager.SurvivalStar / SpawnManager.MAX_STARS) * spawnSettings.maxSpawnRate;
    }

}
