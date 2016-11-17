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
 
        if (carrying && carriedObjectExists) {
            carry(carriedObject);
        }
	}


    void carry(GameObject o) {
        //o.transform.position = Vector3.Lerp(o.transform.position, GameObject.Find("pickupPlace").transform.position, Time.deltaTime * smooth );
        if (GameObject.Find("pickupPlace")) { 
            Vector3 temp = GameObject.Find("pickupPlace").transform.position;
            o.transform.position = new Vector3(temp.x, temp.y, temp.z);
        }
    }


    void OnTriggerEnter(Collider col) {
        
        if(col.gameObject.tag == "pickup" && !carrying) {
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
    }

}

