using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BuildingSpawner))]
public class BuildingSpawnerEditor : Editor
{

    bool isFood = true, isMedical = false, isSurvival = false;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        BuildingSpawner bSpawner = (BuildingSpawner)target;

        bSpawner.debugMode = EditorGUILayout.Toggle("Debug Mode", bSpawner.debugMode);


        if (bSpawner.debugMode)
        {

            bSpawner.hasBuildingOverride = EditorGUILayout.Toggle("Has Building Override", bSpawner.hasBuildingOverride);
            if (bSpawner.hasBuildingOverride)
            {
                bSpawner.buildingOverride = (GameObject)EditorGUILayout.ObjectField("Building Prefab", (GameObject)bSpawner.buildingOverride, typeof(GameObject), true);
            }


            if (bSpawner.IsShop && !bSpawner.hasBuildingOverride)
            {
                bSpawner.hasShopTypeOverride = EditorGUILayout.Toggle("Has Shop Type Override", bSpawner.hasShopTypeOverride);

                if (bSpawner.hasShopTypeOverride)
                {
                    isFood = EditorGUILayout.Toggle("Is a Food Store", isFood);
                    if (isFood)
                    {
                        isMedical = false;
                        isSurvival = false;
                    }
                    isMedical = EditorGUILayout.Toggle("Is a Medical Store", isMedical);
                    if (isMedical)
                    {
                        isSurvival = false;
                        isFood = false;
                    }
                    isSurvival = EditorGUILayout.Toggle("Is a Survival Store", isSurvival);
                    if (isSurvival)
                    {
                        isMedical = false;
                        isFood = false;
                    }

                    if (isFood)
                        bSpawner.shopTypeOverride = 0;
                    if (isMedical)
                        bSpawner.shopTypeOverride = 1;
                    if (isSurvival)
                        bSpawner.shopTypeOverride = 2;
                }
            }
        }
        if (GUI.changed) { EditorUtility.SetDirty(bSpawner); }

    }


}

