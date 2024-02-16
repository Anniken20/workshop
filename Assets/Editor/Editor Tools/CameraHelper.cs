using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CameraController)), CanEditMultipleObjects]
public class CameraHelper : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        CameraController myScript = (CameraController)target;
        if (GUILayout.Button("Reset Camera"))
        {
            myScript.ResetCamera();
        }
    }
}
