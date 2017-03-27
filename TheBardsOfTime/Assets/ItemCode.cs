using UnityEngine;
using System.Collections;

public class ItemCode : MonoBehaviour
{

    Quaternion rot;
    Vector3 Opos;
    Vector3 velocity = Vector3.zero;
    bool goingUP = true;

    // Use this for initialization
    void Start()
    {
        Opos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 1.5f, 0));

        if (goingUP) {
            transform.position = new Vector3(Opos.x, transform.position.y + 0.02f, Opos.z);
            if (transform.position.y > Opos.y + 1)
                goingUP = false;
        } else {
            transform.position = new Vector3(Opos.x, transform.position.y - 0.02f, Opos.z);
            if (transform.position.y < Opos.y)
                goingUP = true;
        }
    }
}
