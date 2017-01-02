using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(PathScript))]
public class PathScriptEditor : Editor {
    public override void OnInspectorGUI()
    {
        PathScript script = (PathScript)target;

        List<GameObject> remove = new List<GameObject>();
        foreach (GameObject point in script.Path)
        {
            EditorGUILayout.BeginHorizontal();
            PathPointScript pointScript = point.GetComponent<PathPointScript>();
            EditorGUILayout.LabelField(pointScript.PointName);
            if (GUILayout.Button("-"))
            {
                remove.Add(point);
            }
            if (GUILayout.Button("Show"))
            {
                Selection.activeGameObject = point;
            }
            EditorGUILayout.EndHorizontal();
        }

        foreach(GameObject obj in remove)
        {
            script.RemovePoint(obj);
        }

        if(GUILayout.Button("Add point"))
        {
            GameObject obj = script.AddPoint();
            Selection.activeGameObject = obj;
        }

    }

}
