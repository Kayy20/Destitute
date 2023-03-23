using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ItemSpawner))]
public class SpawnItemEditor : Editor
{

    bool isFood, isWater, isMedical, isSurvival;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ItemSpawner iSpawner = (ItemSpawner)target;

        switch (iSpawner.itemTypeOverride)
        {
            case ItemSpawner.ItemType.Food:
                isFood = true;
                isWater = false;
                isMedical = false;
                isSurvival = false;
                break;

            case ItemSpawner.ItemType.Medical:
                isFood = false;
                isWater = false;
                isMedical = true;
                isSurvival = false;
                break;

            case ItemSpawner.ItemType.Survival:
                isFood = false;
                isWater=false;
                isMedical = false;
                isSurvival = true;
                break;
        }

        iSpawner.isValuable = EditorGUILayout.Toggle("Is a Valuable", iSpawner.isValuable);

        if (!iSpawner.isValuable)
        {
            iSpawner.FoodChance = EditorGUILayout.Slider("Food Chance", iSpawner.FoodChance, 0f, 1f);
            iSpawner.WaterChance = EditorGUILayout.Slider("Water Chance", iSpawner.WaterChance, 0f, 1f);
            iSpawner.MedChance = EditorGUILayout.Slider("Medical Chance", iSpawner.MedChance, 0f, 1f);
            iSpawner.SurvivalChance = EditorGUILayout.Slider("Survival Chance", iSpawner.SurvivalChance, 0f, 1f);

            if (GUILayout.Button("Reset"))
            {
                iSpawner.FoodChance = 1 / 4.0f;
                iSpawner.WaterChance = 1 / 4.0f;
                iSpawner.MedChance = 1 / 4.0f;
                iSpawner.SurvivalChance = 1 / 4.0f;

            }
            GUILayout.Label("Presets");
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Food/water"))
            {
                iSpawner.FoodChance = 0.5f;
                iSpawner.WaterChance = 0.5f;
                iSpawner.MedChance = 0f;
                iSpawner.SurvivalChance = 0f;
            }
            if (GUILayout.Button("surv/med"))
            {
                iSpawner.FoodChance = 0f;
                iSpawner.WaterChance = 0f;
                iSpawner.MedChance = 0.5f;
                iSpawner.SurvivalChance = 0.5f;
            }
            if (GUILayout.Button("cons/surv/med"))
            {
                iSpawner.FoodChance = 1/6.0f;
                iSpawner.WaterChance = 1/6.0f;
                iSpawner.MedChance = 1/3.0f;
                iSpawner.SurvivalChance = 1/3.0f;
            }
            GUILayout.EndHorizontal();
        }
        
        if(iSpawner.FoodChance != iSpawner.LastVars[0])
        {
            iSpawner.UpdateVars(0);
        }
        else if (iSpawner.MedChance != iSpawner.LastVars[1])
        {
            iSpawner.UpdateVars(1);

        }
        else if (iSpawner.SurvivalChance != iSpawner.LastVars[2])
        {
            iSpawner.UpdateVars(2);

        }
        else if (iSpawner.WaterChance != iSpawner.LastVars[3])
        {
            iSpawner.UpdateVars(3);

        }

        iSpawner.DebugMode = EditorGUILayout.Toggle("Debug Mode", iSpawner.DebugMode);

        if (iSpawner.DebugMode)
        {
           GUILayout.Label("");
           iSpawner.hasFlatSpawnRate = EditorGUILayout.Toggle("Has Flat Spawn Rate", iSpawner.hasFlatSpawnRate);
            if (iSpawner.hasFlatSpawnRate)
            {
                iSpawner.flatSpawnRate = EditorGUILayout.Slider("Flat Spawn Rate", iSpawner.flatSpawnRate, 0f, 1f);
            }

            GUILayout.Label("");
            iSpawner.hasSpawnOverride = EditorGUILayout.Toggle("Has Item Override", iSpawner.hasSpawnOverride);
            if (iSpawner.hasSpawnOverride)
            {
                iSpawner.spawnOverride = (GameObject)EditorGUILayout.ObjectField("Item Prefab", (GameObject)iSpawner.spawnOverride, typeof(GameObject), true);
            }
            else
            {
                GUILayout.Label("");
                iSpawner.hasItemTypeOverride = EditorGUILayout.Toggle("Has Type Override", iSpawner.hasItemTypeOverride);

                if (iSpawner.hasItemTypeOverride)
                {
                    isFood = EditorGUILayout.Toggle("Is a Food Item", isFood);
                    if (isFood)
                    {
                        isMedical = false;
                        isWater=false;
                        isSurvival = false;
                    }
                    isWater = EditorGUILayout.Toggle("Is a Water Item", isWater);
                    if (isWater)
                    {
                        isMedical = false;
                        isFood = false;
                        isSurvival = false;
                    }
                    isMedical = EditorGUILayout.Toggle("Is a Medical Item", isMedical);
                    if (isMedical)
                    {
                        isWater = false;
                        isSurvival = false;
                        isFood = false;
                    }
                    isSurvival = EditorGUILayout.Toggle("Is a Survival Item", isSurvival);
                    if (isSurvival)
                    {
                        isWater = false;        
                        isMedical = false;
                        isFood = false;
                    }
                }

                if (isFood)
                    iSpawner.itemTypeOverride = ItemSpawner.ItemType.Food;
                if (isWater)
                    iSpawner.itemTypeOverride = ItemSpawner.ItemType.Water;
                if (isMedical)
                    iSpawner.itemTypeOverride = ItemSpawner.ItemType.Medical;
                if (isSurvival)
                    iSpawner.itemTypeOverride = ItemSpawner.ItemType.Survival;
                }

        }

        if (GUI.changed) { EditorUtility.SetDirty(iSpawner); }

    }


}
