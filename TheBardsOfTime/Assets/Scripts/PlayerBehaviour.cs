using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

    public float speed;
    public float maxSpeed;
    public bool forward,
                back,
                left,
                right;

    Rigidbody player;

	// Use this for initialization
	void Start ()
    {
        player = GameObject.FindObjectOfType<PlayerBehaviour>().GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.W))
            forward = true;

        else
            forward = false;

        if (Input.GetKey(KeyCode.A))
            left = true;

        else
            left = false;

        if (Input.GetKey(KeyCode.S))
            back = true;

        else
            back = false;

        if (Input.GetKey(KeyCode.D))
            right = true;
        
        else
            right = false;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            Move();

           // Debug.Log(player.velocity);
    }

    void Move()
    {
        if(forward)
        {
            if(player.velocity.magnitude <= maxSpeed)
                player.AddForce(0, 0, speed);
        }

        if (right)
        {
            if(player.velocity.magnitude <= maxSpeed)
                player.AddForce(speed, 0, 0);
        }

        if (back)
        {
            if(player.velocity.magnitude <= maxSpeed)
                player.AddForce(0, 0, -speed);
        }

        if (left)
        {
            if(player.velocity.magnitude <= maxSpeed)
                player.AddForce(-speed, 0, 0);
        }
    }

}
