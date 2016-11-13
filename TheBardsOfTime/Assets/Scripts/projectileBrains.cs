using UnityEngine;
using System.Collections;

public class projectileBrains : MonoBehaviour {

    public GameObject parent;
    public float lifespan = 6f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
        {
            Destroy(parent);
            Debug.Log("Kaboom!");
        }
	}
}
