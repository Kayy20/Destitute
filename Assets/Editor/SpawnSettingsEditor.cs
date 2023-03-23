using UnityEditor;
using UnityEngine;

public class SpawnSettingsEditor : EditorWindow
{
    SpawnSettingsSO spawnSettings;

    protected void OnEnable()
    {
        LoadAsset();
    }

    private void LoadAsset()
    {

        //idek if this will work but hopefully one or both fixes it
        Resources.LoadAll<SpawnSettingsSO>("Assets/Justin/Resources/Settings");
        Resources.LoadAll<SpawnSettingsSO>("Settings");
        SpawnSettingsSO[] spawnArray = Resources.FindObjectsOfTypeAll(typeof(SpawnSettingsSO)) as SpawnSettingsSO[];

        //spawnSettings = Resources.Load<SpawnSettingsSO>("Assets/Justin/Settings/spawnSettings.asset");

        spawnSettings = spawnArray[0];

        Debug.Log(spawnSettings);

        if (spawnSettings == null)
        {
            Debug.Log("FAILED TO LOAD SPAWN SETTINGS ASSET");
            Debug.Log("ATTEMPTING TO CREATE NEW SPAWN SETTINGS ASSET");

            spawnSettings = CreateInstance<SpawnSettingsSO>();
            AssetDatabase.CreateAsset(spawnSettings, "Assets/Justin/Resources/Settings/spawnSettings.asset");

            if (spawnSettings == null)
            {
                Debug.Log("FAILED TO CREATE ASSET. YOURE FUCKED");

            }
            else
            {
                Debug.Log("ASSET SUCCESSFULLY CREATED");

            }

        }
        else
        {
            //youre good :)
            //it spawned

        }
    }

    [MenuItem("Tools/Game Settings/Spawn Settings")]
    public static void ShowWindow()
    {

        EditorWindow.GetWindow<SpawnSettingsEditor>("Spawn Settings");
    }

    private void OnGUI()
    {

        GUILayout.Label("SPAWN SETTINGS\n", EditorStyles.largeLabel);

        if(GUILayout.Button("DELETE ENTIRE PROJECT. DO NOT PRESS"))
        {
            LoadAsset();

        }

        spawnSettings.maxSpawnRate= EditorGUILayout.Slider("Max Spawn Rate", spawnSettings.maxSpawnRate, 0f, 1f);
        spawnSettings.ValuableChance = EditorGUILayout.Slider("Valuable Spawn Rate", spawnSettings.ValuableChance, 0f, 1f);


    }



}
