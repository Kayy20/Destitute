using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public class NewDebugCategory : EditorWindow
{
    private string newCategoryName;
    private CD.Programmers programmer;
    private int sel;
    private Color color;
    private Vector2 scrollPos;
    
    [MenuItem("Tools/Debug/NewDebugCategory")]
    static void Init()
    {
        NewDebugCategory window = (NewDebugCategory) EditorWindow.GetWindow(typeof(NewDebugCategory));
        window.Show();
    }

    private void OnGUI()
    {
        GUILayout.Label("New Debug Category", EditorStyles.largeLabel);
        
        GUILayout.Space(10);
        
        CreateNewCategory();
        
        GUILayout.Space(50);
        
        GUILayout.Label("Categories", EditorStyles.largeLabel);

        GUILayout.Space(10);
        
        PopulateCategoriesList();

    }

    private void CreateNewCategory()
    {
        newCategoryName = EditorGUILayout.TextField("Category Name", newCategoryName);

        EditorGUI.BeginChangeCheck();
        sel = EditorGUILayout.Popup("Programmer:" ,sel, Enum.GetNames(typeof(CD.Programmers)));
        programmer = (CD.Programmers)sel;
        EditorGUI.EndChangeCheck();

        color = EditorGUILayout.ColorField("Color:", color);
        
        if (GUILayout.Button("Confirm"))
        {
            DebugCategories dc = ScriptableObject.CreateInstance<DebugCategories>();
            dc.categoryName = newCategoryName;
            dc.programmer = programmer;
            color.a = 1;
            dc.color = color;
            string path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath($"Assets/Tools/Debug/Resources/DebugCategories/{newCategoryName}.asset");
            AssetDatabase.CreateAsset(dc, path);
            AssetDatabase.SaveAssets();
            
            EditorUtility.FocusProjectWindow();

            Selection.activeObject = dc;
        }
    }

    private void PopulateCategoriesList()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Width(position.width), GUILayout.Height(100));
        
        DebugCategories[] debugCategories = Resources.FindObjectsOfTypeAll(typeof(DebugCategories)) as DebugCategories[];
        
        foreach (DebugCategories category in debugCategories)
        {
            string categoryPath = AssetDatabase.GetAssetPath(category);
            EditorGUILayout.BeginHorizontal();
            
            GUILayout.Space(10);
            var style = new GUIStyle();
            style.normal.textColor = category.color;
            
            GUILayout.Label(category.categoryName, style);
            
            if (GUILayout.Button("Delete", GUILayout.Width(50), GUILayout.Height(20)))
            {
                if (EditorUtility.DisplayDialog($"Deleting: {category.categoryName}",
                    $"Are you sure you want to delete {category.categoryName}?",
                    "Confirm",
                    "Cancel"))
                {
                    AssetDatabase.DeleteAsset(categoryPath);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();
    }
}
#endif