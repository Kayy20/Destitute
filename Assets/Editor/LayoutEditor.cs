using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(LayoutSelector))]
public class LayoutEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LayoutSelector layout = (LayoutSelector)target;

        layout.debugMode = EditorGUILayout.Toggle("Debug Mode", layout.debugMode);

        if (layout.debugMode)
        {
            layout.debugLayout = (GameObject)EditorGUILayout.ObjectField("Layout Override", (GameObject)layout.debugLayout, typeof(GameObject), true);

        }

        if (GUI.changed) { EditorUtility.SetDirty(layout); }
    }


}
