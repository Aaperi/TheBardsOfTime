using UnityEngine;
using System.Collections;

public class CameraBehaviour : MonoBehaviour {

    public GameObject player;
    Vector3 offset;
    

	// Use this for initialization
	void Start ()
    {
        offset = transform.position - player.transform.position;
    }
	
    void Update()
    {
        //transform.position = player.transform.position + offset; Seuraa pelaajaa, mutta rikkoo rotation
        if (Input.GetMouseButton(0))
        {
            transform.RotateAround(player.transform.position, Vector3.up, Input.GetAxis("Mouse X"));
        }

    }

	void LateUpdate ()
    {
        
    }
}
