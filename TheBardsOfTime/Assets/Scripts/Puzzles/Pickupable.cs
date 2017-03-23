using UnityEngine;
using System.Collections;

public class Pickupable : MonoBehaviour
{

    public int weight;
    Vector3 Opos;

    void Start()
    {
        Opos = transform.position;
    }

    void Update()
    {
        if (transform.position.y < -50) {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = Opos;
        }
    }

}
