using UnityEngine;
using System.Collections;

public class harkka : MonoBehaviour {

    [Range(0, 50)]
    public float floatti;

    [Header("Important INT")]
    public int intti;
    [Space]

    [Tooltip("Only set true if not false")]
    public bool booli;

    [TextArea]
    public string lonkka;

    public harkka2.ItemType ItemType;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
