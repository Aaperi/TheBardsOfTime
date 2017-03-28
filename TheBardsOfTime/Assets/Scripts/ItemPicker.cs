using UnityEngine;
using System.Collections;

public class ItemPicker : MonoBehaviour {

    int notes = 0;

	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Item") && col.gameObject != null) {
            notes++;
            Debug.Log("You got " + notes + " notes");
        }
        Destroy(col.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
