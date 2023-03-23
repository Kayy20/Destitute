using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    
    List<ItemSO> foods, waters, medicals, survivals, valuables;

    public Color foodColour = Color.blue, watColour = Color.blue, medColour = Color.red, survColour = Color.green, valColour = Color.yellow;

    private static ItemManager instance;
    public static ItemManager Instance
    {
        get { return instance; }
    }

    [SerializeField]
    private float outlineDist = 5;
    public float sqrOutlineDist { get;private set; }

    private void Awake()
    {

        foods = new List<ItemSO>();
        waters = new List<ItemSO>();
        medicals = new List<ItemSO>();
        survivals = new List<ItemSO>();
        valuables = new List<ItemSO>();
        
        sqrOutlineDist = outlineDist*outlineDist;
        
        Resources.LoadAll<ItemSO>("Items");
        ItemSO[] items = Resources.FindObjectsOfTypeAll(typeof(ItemSO)) as ItemSO[];


        for (int i=0; i<items.Length;i++){
            switch (items[i].resType)
            {
                case AnswerType.ResourceType.Food:
                    foods.Add(items[i]);
                    break;
                case AnswerType.ResourceType.Water:
                    waters.Add(items[i]);
                    break;
                case AnswerType.ResourceType.Medical:
                    medicals.Add(items[i]);
                    break;
                case AnswerType.ResourceType.Survival:
                case AnswerType.ResourceType.GunStuff:
                case AnswerType.ResourceType.SharpEdges:
                    survivals.Add(items[i]);
                    break;
                case AnswerType.ResourceType.Valuable:
                    valuables.Add(items[i]);
                    break;
            }
        }

        instance = this;

    }

    public GameObject GetFood()
    {
        return foods[Random.Range(0, foods.Count)].prefab;
    }
    public GameObject GetWater()
    {
        return waters[Random.Range(0, waters.Count)].prefab;
    }
    public GameObject GetMedical()
    {
        return medicals[Random.Range(0, medicals.Count)].prefab;
    }
    public GameObject GetSurvival()
    {
        return survivals[Random.Range(0, survivals.Count)].prefab;
    }
    public GameObject GetValuable()
    {
        return valuables[Random.Range(0, valuables.Count)].prefab;
    }

}

