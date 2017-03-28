using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour
{
    Quaternion rot;
    Vector3 Opos;
    Transform CT;
    GameManager gm;
    bool goingUP = true;
    bool pickd = false;


    void Start()
    {
        CT = gameObject.transform.GetChild(0).transform;
        gm = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        Opos = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        CT.Rotate(new Vector3(0, 1.5f, 0));

        if (goingUP) {
            CT.position = new Vector3(Opos.x, CT.position.y + 0.02f, Opos.z);
            if (CT.position.y > Opos.y + 1.5f)
                goingUP = false;
        } else {
            CT.position = new Vector3(Opos.x, CT.position.y - 0.02f, Opos.z);
            if (CT.position.y < Opos.y)
                goingUP = true;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag.Equals("Player") && !pickd) {
            gm.notes++;
            pickd = true;
            Destroy(gameObject);
        }
    }
}
