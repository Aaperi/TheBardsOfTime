using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PathScript))]
public class PathScriptEditor : Editor {
    public override void OnInspectorGUI()
    {
        // Get a reference to the script
        PathScript script = (PathScript)target;

        // Loop through points in the path and render a GUI for each
        // The GUI contains a Label with the name of the point,
        // a Button for removing the point and a Button for setting
        // the point as selected in the Scene
        List<GameObject> remove = new List<GameObject>();
        foreach(GameObject point in script.Path)
        {
            EditorGUILayout.BeginHorizontal();
            PathPointScript pointScript = point.GetComponent<PathPointScript>();
            EditorGUILayout.LabelField(pointScript.PointName);
            if (GUILayout.Button("Remove"))
            {
                remove.Add(point);
            }
            if (GUILayout.Button("Show"))
            {
                Selection.activeGameObject = point;
            }
            EditorGUILayout.EndHorizontal();
        }

        // Remove the points in th remove-list
        foreach(GameObject obj in remove)
        {
            script.RemovePoint(obj);
        }

        // If the user clicks the Add-button add a new point to the path
        // and set it as the active object in the scene
        if(GUILayout.Button("Add point"))
        {
            GameObject obj = script.AddPoint();
            Selection.activeGameObject = obj;
        }
    }
}
