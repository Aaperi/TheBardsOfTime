using UnityEngine;
using UnityEditor;
using System.Collections;

public class MenuItems : Editor {
    [MenuItem("Tools/Add Path")]
    private static void AddPath()
    {
        GameObject obj = new GameObject("Path");
        obj.AddComponent<PathScript>();
    }
}
