using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateWellsAndLevels))]
public class CreateWellsAndLevelsInspector : Editor
{
    // Custom inspector for the CreateWellsAndLevels script.
    public override void OnInspectorGUI()
    {
        // Draws the default inspector.
        DrawDefaultInspector();

        CreateWellsAndLevels script = (CreateWellsAndLevels)target;

        EditorGUILayout.Space();

        // Creates a UI button that calls the build method in the CreateWellsAndLevels script.
        if (GUILayout.Button("Build wells and levels"))
        {
            script.Build();
        }
    }
}
