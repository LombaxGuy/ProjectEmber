using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CreateWellsAndLevels))]
public class CreateWellsAndLevelsInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CreateWellsAndLevels script = (CreateWellsAndLevels)target;

        if (GUILayout.Button("Build wells and levels"))
        {
            script.Build();
        }
    }
}
