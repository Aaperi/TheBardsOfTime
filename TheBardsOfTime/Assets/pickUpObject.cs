using UnityEngine;
using System.Collections;

public class pickUpObject : MonoBehaviour {

    private bool carrying;
    private float smooth;
    private GameObject carriedObject;
    private MenuScript MSref;

    void Start()
    {
        MSref = FindObjectOfType<MenuScript>();
    }

	void Update () {

        if (Input.GetKeyDown(KeyCode.F) && carriedObject != null) {
            if (!carrying) {
                PickUp();
            } else {
                Drop();
            }
        }
 
        if (carrying && carriedObject != null) {
            Carry();
        }

        if(!carrying && !MSref.actionGuide.activeSelf && carriedObject != null) {
            MSref.SendMessage("ShowGuide", "pickup " + carriedObject.name);
        }
    }


    void Carry() {
        if (GameObject.Find("pickupPlace")) { 
            Vector3 temp = GameObject.Find("pickupPlace").transform.position;
            carriedObject.transform.position = new Vector3(temp.x, temp.y, temp.z);
        }
    }


    void OnTriggerEnter(Collider col) {
        if(col.gameObject.tag == "pickup" && !carrying) {
            carriedObject = col.gameObject;
        }
    }

    void OnTriggerExit(Collider col) {
        if(col.gameObject == carriedObject && !carrying) {
            carriedObject = null;
        }
        MSref.SendMessage("HideGuide");
    }

    void Drop()
    {
        carrying = false;
        carriedObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    void PickUp()
    {
        carrying = true;
        carriedObject.GetComponent<Rigidbody>().isKinematic = true;
        MSref.SendMessage("HideGuide");
    }
}

