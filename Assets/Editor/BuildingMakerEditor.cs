using UnityEngine;
using UnityEditor;

public class BuildingMakerEditor : EditorWindow
{

    bool customName = false;

    string buildingName;
    GameObject buildingPrefab;
    bool isShop;

    SO_House soHouse;
    SO_Shop soShop;

    SO_Shop.ShopType shopType;
    
    bool isFood = true;
    bool isMedical = false;
    bool isSurvival = false;

    private void CreateAsset()
    {
       

        if (isShop)
        {
            SO_Shop building = CreateInstance<SO_Shop>();
            building.BuildingPrefab = buildingPrefab;

            if (isFood)
                building.sType = SO_Shop.ShopType.Food; 
            if (isMedical)
                building.sType = SO_Shop.ShopType.Medical; 
            if (isSurvival)
                building.sType = SO_Shop.ShopType.Survival;

            AssetDatabase.CreateAsset(building, "Assets/Justin/Resources/Houses/" + buildingName + ".asset");
            AssetDatabase.SaveAssets();
            
            buildingName = "";
            buildingPrefab = null;
            isShop = false;
            customName = false;

            isFood = true;
            isMedical = false;
            isSurvival = false;

        }
        else
        {
            SO_House building = CreateInstance<SO_House>();
            building.BuildingPrefab = buildingPrefab;

            AssetDatabase.CreateAsset(building, "Assets/Justin/Resources/Houses/" + buildingName + ".asset");
            AssetDatabase.SaveAssets();

            buildingName = "";
            buildingPrefab = null;
            isShop = false;
            customName = false;

            isFood = true;
            isMedical = false;
            isSurvival = false;
        }


       

    }

    [MenuItem("Tools/Buildings/Create _b",false,0)]
    public static void ShowWindow()
    {

        EditorWindow.GetWindow<BuildingMakerEditor>("Create Building");
    }

    private void OnGUI()
    {

        GUILayout.Label("CREATE NEW BUILDING\n", EditorStyles.largeLabel);

        customName = EditorGUILayout.Toggle("Custom Name", customName);
        if(customName)
            buildingName = EditorGUILayout.TextField("Building Name", buildingName);
        
        buildingPrefab = (GameObject)EditorGUILayout.ObjectField("Building Prefab", (GameObject)buildingPrefab, typeof(GameObject), true);

        if(buildingPrefab!=null && !customName)
        {
            buildingName = buildingPrefab.name;
        }
        
        isShop = EditorGUILayout.Toggle("Is a Shop", isShop);
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

        if (GUILayout.Button("Create Building"))
        {

            if (!customName)
            {
                buildingName = buildingPrefab.name;
            }


            if (buildingName == "")
            {
                Debug.LogError("NO NAME ENTERED");
            }else if (!buildingPrefab)
            {
                Debug.LogError("NO BUILDING PREFAB ENTERED");
            }
            else
            {
                CreateAsset();
            }
        }

       

    }


    public string GenerateHouseName()
    {

        SO_House[] houseArray = Resources.FindObjectsOfTypeAll(typeof(SO_House)) as SO_House[];

        return "House " + houseArray.Length;
    }



    public string GenerateShopName()
    {
        SO_Shop[] shopArray = Resources.FindObjectsOfTypeAll(typeof(SO_Shop)) as SO_Shop[];

        return "Shop " + shopArray.Length;

    }

    
}
