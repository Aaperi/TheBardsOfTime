using UnityEngine;
using System.Collections;

public class pickUpObject : MonoBehaviour {

    bool carrying;
    GameObject carriedObject;
    public float smooth;
    bool carriedObjectExists = false;

	void Update () {
        if (Input.GetKeyDown(KeyCode.E) && carriedObjectExists) {
            if (!carrying) {
                carrying = true;
                carriedObject.GetComponent<Rigidbody>().isKinematic = true;
            } else {
                dropObject();
            }
        }
 
        if (carrying) {
            carry(carriedObject);
        }
	}

    void carry(GameObject o) {
        if(carriedObjectExists)
        o.transform.position = Vector3.Lerp(o.transform.position, GameObject.Find("pickupPlace").transform.position, Time.deltaTime * smooth );
    }


    void OnTriggerEnter(Collider col) {
        
        if(col.gameObject.tag == "pickup") {
            carriedObjectExists = true;
        carriedObject = col.gameObject;
        }
    }

    void OnTriggerExit(Collider col) {
        if(col.gameObject == carriedObject && !carrying) {
            carriedObject = null;
            carriedObjectExists = false;
        }
    }


    void dropObject() {
        carrying = false;
        carriedObject.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        carriedObject = null;
    }

}

