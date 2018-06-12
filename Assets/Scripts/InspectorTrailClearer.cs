using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(SwipeTrail))]
public class InspectorTrailClearer : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SwipeTrail myScript = (SwipeTrail)target;
        if (GUILayout.Button("Clear Trail"))
        {
            myScript.clearTrail();
        }
    }
}