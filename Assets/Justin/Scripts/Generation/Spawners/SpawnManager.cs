using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager
{

    private static int foodStar = 5;
    public static int FoodStar
    {
        get { return foodStar; }
    }

    private static int medicalStar = 5;
    public static int MedicalStar
    {
        get { return medicalStar; }
    }

    private static int survivalStar = 5;
    public static int SurvivalStar
    {
        get { return survivalStar; }
    }

    public static int MAX_STARS
    {
        get { return 5; }
    }


    public static void SetSpawnSettings(Node townNode)
    {
        foodStar = townNode.Information.Food;
        medicalStar = townNode.Information.Medical;
        survivalStar = townNode.Information.Survival;

    }

}
