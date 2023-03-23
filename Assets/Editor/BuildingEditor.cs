using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BuildingEditor : EditorWindow
{

    static SO_House[] houseArray;
    static SO_Shop[] shopArray;

    Editor prefabPreviewEditor;

    string[] BuildingNames
    {
        get
        {
            string[] names;
            if (isShop)
            {
                names = new string[shopArray.Length];

                for (int i = 0; i < shopArray.Length; i++)
                {
                    names[i] = shopArray[i].name;
                }
            }
            else
            {
                names = new string[houseArray.Length];

                for(int i=0; i < houseArray.Length; i++)
                {
                    names[i] = houseArray[i].name;
                }

            }

            return names;
        }
    }

    bool lastShop = true;
    bool isShop=false;
    int lastNum=-1;
    int editingBuildingNum=0;

    string buildingName;
    GameObject buildingPrefab;

    bool isFood, isMedical, isSurvival;

    [MenuItem("Tools/Buildings/Edit #b",false,1)]
    public static void ShowWindow()
    {
        Resources.LoadAll<SO_Building>("Houses");
     
        houseArray = Resources.FindObjectsOfTypeAll(typeof(SO_House)) as SO_House[];
        shopArray = Resources.FindObjectsOfTypeAll(typeof(SO_Shop)) as SO_Shop[];

        EditorWindow.GetWindow<BuildingEditor>("Edit Building");


    }


    private void OnGUI()
    {

        GUILayout.Label("Edit Building");

        GUILayout.BeginHorizontal();

        isShop = !GUILayout.Toggle(!isShop,"House");
        GUILayout.Label("\t");
        isShop = GUILayout.Toggle(isShop, "Shop");

        GUILayout.EndHorizontal();
        GUILayout.Space(25);

        if (isShop != lastShop)
            editingBuildingNum = 0;

        editingBuildingNum = EditorGUILayout.Popup("Building: ", editingBuildingNum, BuildingNames);

        GUILayout.Space(10);
        buildingName = EditorGUILayout.TextField("Custom Name", buildingName);

        buildingPrefab = (GameObject)EditorGUILayout.ObjectField("Building Prefab", (GameObject)buildingPrefab, typeof(GameObject), true);
        UpdateBuildingInfo();

        if (isShop)
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
        }
        
        string path = AssetDatabase.GetAssetPath(buildingPrefab);
        
        GUIStyle bgColor = new GUIStyle();
        bgColor.normal.background = EditorGUIUtility.whiteTexture;

        if (buildingPrefab != null)
        {
            Debug.Log(path);
            if(prefabPreviewEditor)
            {
                DestroyImmediate(prefabPreviewEditor);
                prefabPreviewEditor= Editor.CreateEditor(buildingPrefab);
                prefabPreviewEditor.Repaint();
            }
            else
            {
                prefabPreviewEditor = Editor.CreateEditor(buildingPrefab);

            }

            prefabPreviewEditor.OnInteractivePreviewGUI(GUILayoutUtility.GetRect(250, 250), bgColor);
        }
        
        SaveBuildingInfo();

        GUILayout.Space(10);
        if (GUILayout.Button("SAVE"))
        {
            SaveBuildingInfo();
        }
        if (GUILayout.Button("DELETE"))
        {
            DeleteBuildingForever();
        }

    }

    private void UpdateBuildingInfo()
    {
        if (lastNum != editingBuildingNum || lastShop!=isShop)
        {

            if (isShop)
            {
                //shops
                buildingName = shopArray[editingBuildingNum].name;
                buildingPrefab = shopArray[editingBuildingNum].BuildingPrefab;

                switch (shopArray[editingBuildingNum].sType)
                {
                    case SO_Shop.ShopType.Food:
                        isFood = true;
                        isMedical = false;
                        isSurvival = false;
                        break;
                    case SO_Shop.ShopType.Medical:
                        isFood = false;
                        isMedical = true;
                        isSurvival = false;
                        break;
                    case SO_Shop.ShopType.Survival:
                        isFood = false;
                        isMedical = false;
                        isSurvival = true;
                        break;
                }

            }
            else
            {
                //buildings
                buildingName = houseArray[editingBuildingNum].name;
                buildingPrefab = houseArray[editingBuildingNum].BuildingPrefab;

            }

            lastNum = editingBuildingNum;
            lastShop = isShop;
        }
        

    }

    private void SaveBuildingInfo()
    {

        if (isShop)
        {
            //save data for a shop
           shopArray[editingBuildingNum].BuildingPrefab = buildingPrefab;
           shopArray[editingBuildingNum].name = buildingName;

            if (isFood)
            {
                shopArray[editingBuildingNum].sType = SO_Shop.ShopType.Food;
            }else if (isMedical)
            {
                shopArray[editingBuildingNum].sType = SO_Shop.ShopType.Medical;
            }
            else
            {
                shopArray[editingBuildingNum].sType = SO_Shop.ShopType.Survival;
            }

        }
        else
        {
            //save data for a house
            houseArray[editingBuildingNum].BuildingPrefab = buildingPrefab;
            houseArray[editingBuildingNum].name = buildingName;
        }

    }

    private void ForceUpdateBuildingInfo()
    {
        if (isShop)
            {
                //shops
                buildingName = shopArray[editingBuildingNum].name;
                buildingPrefab = shopArray[editingBuildingNum].BuildingPrefab;

                switch (shopArray[editingBuildingNum].sType)
                {
                    case SO_Shop.ShopType.Food:
                        isFood = true;
                        isMedical = false;
                        isSurvival = false;
                        break;
                    case SO_Shop.ShopType.Medical:
                        isFood = false;
                        isMedical = true;
                        isSurvival = false;
                        break;
                    case SO_Shop.ShopType.Survival:
                        isFood = false;
                        isMedical = false;
                        isSurvival = true;
                        break;
                }

            }
            else
            {
                //buildings
                buildingName = houseArray[editingBuildingNum].name;
                buildingPrefab = houseArray[editingBuildingNum].BuildingPrefab;

            }

            lastNum = editingBuildingNum;
            lastShop = isShop;
        


    }


    private void DeleteBuildingForever()
    {

       
        string path;

        if (isShop)
        {
            path=AssetDatabase.GetAssetPath(shopArray[editingBuildingNum]);
            Resources.UnloadAsset(shopArray[editingBuildingNum]);

        }
        else
        {
            path=AssetDatabase.GetAssetPath(houseArray[editingBuildingNum]);
            Resources.UnloadAsset(houseArray[editingBuildingNum]);
        }
        File.Delete(path);

        path += ".meta";
        File.Delete(path);
        
        editingBuildingNum = 0;
        
        Resources.LoadAll<SO_Building>("Houses");

        houseArray = Resources.FindObjectsOfTypeAll(typeof(SO_House)) as SO_House[];
        shopArray = Resources.FindObjectsOfTypeAll(typeof(SO_Shop)) as SO_Shop[];

        ForceUpdateBuildingInfo();
    }

    private void OnDestroy()
    {
        if (areChangesMade())
        {
            //open a confirmation menu
            BuildingSaveOnQuitEditor.buildingPrefab = buildingPrefab;
            BuildingSaveOnQuitEditor.buildingName = buildingName;
            BuildingSaveOnQuitEditor.isShop = isShop;
            if (isShop)
            {
                BuildingSaveOnQuitEditor.isFood = isFood;
                BuildingSaveOnQuitEditor.isMedical = isMedical;
                BuildingSaveOnQuitEditor.isSurvival = isSurvival;

                BuildingSaveOnQuitEditor.shop = shopArray[editingBuildingNum];
            }
            else
            {
                BuildingSaveOnQuitEditor.house = houseArray[editingBuildingNum];
            }
            EditorWindow.GetWindow<BuildingSaveOnQuitEditor>("HOLD UP THERE BUDDY!!!");

        }
    }

    bool areChangesMade()
    {

        if (isShop)
        {
            if (!buildingName.Equals(shopArray[editingBuildingNum].name))
            {
                return true;
            }
            if (!buildingPrefab.Equals(shopArray[editingBuildingNum].BuildingPrefab))
            {
                return true;
            }

            switch (shopArray[editingBuildingNum].sType)
            {
                case SO_Shop.ShopType.Food:
                    if (!isFood)
                        return true;
                    break;
                case SO_Shop.ShopType.Medical:
                    if (!isMedical)
                        return true;
                    break;
                case SO_Shop.ShopType.Survival:
                    if (!isSurvival)
                        return true;
                    break;
            }

        }
        else
        {
            if (!buildingName.Equals(houseArray[editingBuildingNum].name))
            {
                return true;
            }
            if (!buildingPrefab.Equals(houseArray[editingBuildingNum].BuildingPrefab))
            {
                return true;
            }
        }

        return false;
    }

}


public class BuildingSaveOnQuitEditor : EditorWindow
{

    public static bool isShop;
    public static SO_House house;
    public static SO_Shop shop;

    public static string buildingName;

    public static GameObject buildingPrefab;

    public static bool isFood, isMedical, isSurvival;


    private void OnGUI()
    {

        GUILayout.Label("Oh. You didnt save that.");
        GUILayout.Label("");
        GUILayout.Label("CHANGES:");
        if (isShop)
        {
            GUILayout.Label("Building Name: "+shop.name +" > " + buildingName);
            GUILayout.Label("Building Prefab: "+shop.BuildingPrefab +" > " + buildingPrefab);

            if (isFood)
            {
                GUILayout.Label("Shop Type: " + shop.sType + " > Food");

            }
            if (isMedical)
            {
                GUILayout.Label("Shop Type: " + shop.sType + " > Medical");

            }
            if (isSurvival)
            {
                GUILayout.Label("Shop Type: " + shop.sType + " > Survival");

            }

        }
        else
        {
            GUILayout.Label("Building Name: " + house.name + " > " + buildingName);
            GUILayout.Label("Building Prefab: " + house.BuildingPrefab + " > " + buildingPrefab);
        }
       
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Close Anyway"))
        {
            Close();
        }
        if (GUILayout.Button("Save"))
        {
            DoTheSave();
            Close();
        }
        GUILayout.EndHorizontal();


    }
    private void DoTheSave()
    {
        if (isShop)
        {
            shop.BuildingPrefab = buildingPrefab;

            shop.name = buildingName;

            if (isFood)
                shop.sType = SO_Shop.ShopType.Food;
            if (isMedical)
                shop.sType = SO_Shop.ShopType.Medical;
            if (isSurvival)
                shop.sType = SO_Shop.ShopType.Survival;

        }
        else
        {
            house.BuildingPrefab = buildingPrefab;
            house.name = buildingName;

        }
    }
}
