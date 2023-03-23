using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CompositeScript))]
public class CompositeBehaviourEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Setup
        CompositeScript cb = (CompositeScript)target;

        Rect r = EditorGUILayout.BeginHorizontal();
        r.height = EditorGUIUtility.singleLineHeight;
        EditorGUILayout.BeginVertical();
        // check for behaviours
        if (cb.behaviours == null || cb.behaviours.Length == 0)
        { // No behaviours found
            EditorGUILayout.HelpBox("No behaviours in array.", MessageType.Warning);
            EditorGUILayout.EndHorizontal();
            r = EditorGUILayout.BeginHorizontal();
            r.height = EditorGUIUtility.singleLineHeight;
        }
        else
        {
            r.x = 30f;
            r.width = EditorGUIUtility.currentViewWidth - 95f;
            EditorGUI.LabelField(r, "Behaviours");
            r.x = EditorGUIUtility.currentViewWidth - 65f;
            r.width = 60f;
            EditorGUI.LabelField(r, "Weights");

            r.y += EditorGUIUtility.singleLineHeight * 1.2f;
            GUILayout.Space(20);

            EditorGUI.BeginChangeCheck();
            for (int i = 0; i < cb.behaviours.Length; i++)
            {
                r.x = 5f;
                r.width = 20f;
                EditorGUI.LabelField(r, i.ToString());

                r.x = 30f;
                r.width = EditorGUIUtility.currentViewWidth - 95f;
                cb.behaviours[i] = (FlockBehaviour)EditorGUI.ObjectField(r, cb.behaviours[i], typeof(FlockBehaviour), false);

                r.x = EditorGUIUtility.currentViewWidth - 65f;
                r.width = 60f;
                cb.weights[i] = EditorGUI.FloatField(r, cb.weights[i]);

                r.y += EditorGUIUtility.singleLineHeight * 1.1f;
                GUILayout.Space(20);

            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(cb);
            }
        }
        
        r.x = 5f;
        r.width = EditorGUIUtility.currentViewWidth - 10f;
        r.y += EditorGUIUtility.singleLineHeight * 0.5f;
        GUILayout.Space(10);

        if (GUILayout.Button("Add Behaviour"))
        {
            // Add behaviour
            AddBehaviour(cb);
            EditorUtility.SetDirty(cb);
        }

        r.y += EditorGUIUtility.singleLineHeight * 1.5f;
        GUILayout.Space(5);
        if (cb.behaviours != null && cb.behaviours.Length > 0)
            if (GUILayout.Button("Remove Behaviour"))
            {
                // Remove Behaviour
                RemoveBehaviour(cb);
                EditorUtility.SetDirty(cb);
            }
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();
    }

    void AddBehaviour(CompositeScript cb)
    {
        int oldCount = cb.behaviours != null ? cb.behaviours.Length : 0;
        FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount + 1];
        float[] newWeights = new float[oldCount + 1];
        for (int i = 0; i < oldCount; i++)
        {
            newBehaviours[i] = cb.behaviours[i];
            newWeights[i] = cb.weights[i];
        }

        newWeights[oldCount] = 1f;
        cb.behaviours = newBehaviours;
        cb.weights = newWeights;

    }

    void RemoveBehaviour(CompositeScript cb)
    {
        int oldCount = cb.behaviours.Length;

        if (oldCount == 1)
        {
            cb.behaviours = null;
            cb.weights = null;
            return;
        }

        FlockBehaviour[] newBehaviours = new FlockBehaviour[oldCount - 1];
        float[] newWeights = new float[oldCount - 1];
        for (int i = 0; i < oldCount - 1; i++)
        {
            newBehaviours[i] = cb.behaviours[i];
            newWeights[i] = cb.weights[i];
        }

        cb.behaviours = newBehaviours;
        cb.weights = newWeights;
    }

}
