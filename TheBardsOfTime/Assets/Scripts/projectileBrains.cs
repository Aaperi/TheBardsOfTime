using UnityEngine;
using System.Collections;

public class projectileBrains : MonoBehaviour {

    public float lifespan = 3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        lifespan -= Time.deltaTime;
        if (lifespan <= 0)
        {
			Destroy(gameObject);
            Debug.Log("Kaboom!");
        }
	}
}
