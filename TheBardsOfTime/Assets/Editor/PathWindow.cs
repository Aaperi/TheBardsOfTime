using UnityEngine;
using UnityEditor;
using System.Collections;

public class PathWindow : EditorWindow {

    [MenuItem("Tools/PathWindow")]
    static void Init()
    {
        PathWindow window = (PathWindow)EditorWindow.GetWindow(typeof(PathWindow));
        window.Show();
    }

    void OnGUI()
    {
        PathScript[] paths = GameObject.FindObjectsOfType<PathScript>();
        foreach(PathScript path in paths)
        {
            EditorGUILayout.BeginHorizontal();

            bool toggled = EditorGUILayout.Toggle(path.PathEnabled);
            if(toggled != path.PathEnabled)
            {
                path.PathEnabled = toggled;
                EditorUtility.SetDirty(path.gameObject);
            }
            EditorGUILayout.LabelField(path.gameObject.name);
            EditorGUILayout.EndHorizontal();
        }
    }
}
