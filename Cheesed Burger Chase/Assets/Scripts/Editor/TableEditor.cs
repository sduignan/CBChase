using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TableController))]
public class TableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
        serializedObject.Update();

        base.OnInspectorGUI();
        if(GUILayout.Button("Resize Table"))
        {
            var tabler = target as TableController;
            tabler.ResizeTable();
        }

        // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
        serializedObject.ApplyModifiedProperties();
    }
}
