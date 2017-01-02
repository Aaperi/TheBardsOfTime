using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CustomPropertyDrawer(typeof(harkka2.ItemType))]
public class ItemTypeDrawer : PropertyDrawer {

    public int selectedIdex = 0;
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        harkka2.ItemType[] types = (harkka2.ItemType[])Enum.GetValues(typeof(harkka2.ItemType));
        string[] itemTypes = new string[types.Length];
        for (int i = 0; i < itemTypes.Length; i++)
        {
            string path = types[i].ToString();
            if ((int)types[i] < 100)
                path = "Weapon/" + path;
            else if ((int)types[i] < 300)
                path = "Food/" + path;
            else
                path = "Other/" + path;
            itemTypes[i] = path;
        }

        selectedIdex = EditorGUI.Popup(position, selectedIdex, itemTypes);

        EditorGUI.EndProperty();
    }

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
